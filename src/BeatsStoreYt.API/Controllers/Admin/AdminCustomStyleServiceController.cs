using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Services;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.DTOs.Services;
using BeatsStoreYt.API.Services.Admin;
using BeatsStoreYt.API.Services.Notifications;
using BeatsStoreYt.API.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/admin/v1/services/custom-style")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminCustomStyleServiceController : BaseAdminController
{
    private static readonly string[] AllowedExtensions = [".inf", ".info"];
    private static readonly string[] AllowedContentTypes = ["text/plain", "application/octet-stream", "application/x-inf", "text/inf"];

    private readonly BeatsStoreDbContext _context;
    private readonly IAzureBlobStorageService _blobStorage;
    private readonly IEmailService _emailService;

    public AdminCustomStyleServiceController(BeatsStoreDbContext context, IAzureBlobStorageService blobStorage, IEmailService emailService)
    {
        _context = context;
        _blobStorage = blobStorage;
        _emailService = emailService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetOpenRequests(CancellationToken ct = default)
    {
        var requests = await _context.CustomStyleRequests
            .AsNoTracking()
            .Where(r => r.Status == CustomStyleRequestStatus.Pending || r.Status == CustomStyleRequestStatus.InProgress)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new
            {
                r.Id,
                r.OrderId,
                r.UserId,
                status = r.Status.ToString(),
                r.UserUploadUrl,
                r.AdminProcessedUrl,
                r.CreatedAt,
                r.UpdatedAt
            })
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Success(new { requests }, "בקשות פתוחות נטענו בהצלחה"));
    }

    [HttpPut("{id:int}/upload-result")]
    [RequestSizeLimit(100_000_000)]
    public async Task<ActionResult<ApiResponse<object>>> UploadResult(
        int id,
        [FromForm] IFormFile file,
        CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            return BadRequest(ApiResponse<object>.Failure("לא נבחר קובץ"));

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return BadRequest(ApiResponse<object>.Failure("רק קבצי .inf או .info מותרים"));

        if (!IsAllowedMimeType(file.ContentType))
            return BadRequest(ApiResponse<object>.Failure("סוג קובץ לא תקין. רק קבצי INF/INFO מותרים"));

        var styleRequest = await _context.CustomStyleRequests.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (styleRequest is null)
            return NotFound(ApiResponse<object>.Failure("בקשה לא נמצאה"));

        var safeName = Path.GetFileName(file.FileName);
        var blobPath = $"custom-style/admin-results/{styleRequest.UserId}/{styleRequest.OrderId}/{safeName}";

        await using var stream = file.OpenReadStream();
        var storedPath = await _blobStorage.UploadFileAsync(new BlobFileUploadRequest
        {
            BlobPath = blobPath,
            ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            Content = stream
        }, ct);

        styleRequest.AdminProcessedUrl = storedPath;
        styleRequest.Status = CustomStyleRequestStatus.Completed;
        styleRequest.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(ct);

        await WriteAdminAuditAsync("CUSTOM_STYLE_UPLOAD_RESULT", "CustomStyleRequest", styleRequest.Id.ToString(), null, new { storedPath }, ct);

        return Ok(ApiResponse<object>.Success(new { styleRequest.Id, styleRequest.AdminProcessedUrl, status = styleRequest.Status.ToString() }, "קובץ מעובד הועלה בהצלחה"));
    }

    [HttpPost("{id:int}/comments")]
    public async Task<ActionResult<ApiResponse<object>>> AddAdminComment(
        int id,
        [FromBody] AddCustomStyleCommentRequestDto request,
        CancellationToken ct = default)
    {
        var styleRequest = await _context.CustomStyleRequests
            .Include(r => r.Comments)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (styleRequest is null)
            return NotFound(ApiResponse<object>.Failure("בקשה לא נמצאה"));

        if (string.IsNullOrWhiteSpace(request.Content))
            return BadRequest(ApiResponse<object>.Failure("תוכן הערה לא יכול להיות ריק"));

        var sender = User.FindFirstValue(ClaimTypes.Name) ?? "Admin";

        styleRequest.Comments.Add(new Comment
        {
            SenderName = sender,
            Content = request.Content.Trim(),
            IsFromAdmin = true,
            CreatedAt = DateTimeOffset.UtcNow
        });

        if (styleRequest.Status == CustomStyleRequestStatus.Pending)
            styleRequest.Status = CustomStyleRequestStatus.InProgress;

        styleRequest.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(ct);

        var customerEmail = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == styleRequest.UserId)
            .Select(u => u.Email)
            .FirstOrDefaultAsync(ct);

        if (!string.IsNullOrWhiteSpace(customerEmail))
        {
            await _emailService.SendAsync(
            customerEmail,
            "קיבלת הודעה חדשה בבקשת השירות שלך",
            $"קיבלת הודעה חדשה בבקשת השירות שלך (מספר {styleRequest.Id}).",
            ct);
        }

        await WriteAdminAuditAsync("CUSTOM_STYLE_ADD_ADMIN_COMMENT", "CustomStyleRequest", styleRequest.Id.ToString(), null, new { request.Content }, ct);

        return Ok(ApiResponse<object>.Success(new { styleRequest.Id }, "הערת מנהל נוספה בהצלחה"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken ct = default)
    {
        var styleRequest = await _context.CustomStyleRequests
            .Include(r => r.Comments)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (styleRequest is null)
            return NotFound(ApiResponse<object>.Failure("בקשה לא נמצאה"));

        _context.CustomStyleRequests.Remove(styleRequest);
        await _context.SaveChangesAsync(ct);
        await WriteAdminAuditAsync("CUSTOM_STYLE_DELETE_REQUEST", "CustomStyleRequest", id.ToString(), styleRequest, null, ct);

        return Ok(ApiResponse<object>.Success(new { id }, "בקשה נמחקה בהצלחה"));
    }

    private static bool IsAllowedMimeType(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return false;

        return AllowedContentTypes.Contains(contentType.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
