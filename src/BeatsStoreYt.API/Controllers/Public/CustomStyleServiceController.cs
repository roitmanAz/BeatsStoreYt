using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Commerce.Common;
using BeatsStoreYt.API.Data.Features.Services;
using BeatsStoreYt.API.DTOs.Services;
using BeatsStoreYt.API.Services.Admin;
using BeatsStoreYt.API.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/services/custom-style")]
[Authorize]
public class CustomStyleServiceController : ControllerBase
{
    private static readonly string[] AllowedExtensions = [".inf", ".info"];
    private static readonly string[] AllowedContentTypes = ["text/plain", "application/octet-stream", "application/x-inf", "text/inf"];

    private readonly BeatsStoreDbContext _context;
    private readonly IAzureBlobStorageService _blobStorage;
    private readonly IAuditLogService _audit;

    public CustomStyleServiceController(BeatsStoreDbContext context, IAzureBlobStorageService blobStorage, IAuditLogService audit)
    {
        _context = context;
        _blobStorage = blobStorage;
        _audit = audit;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(100_000_000)]
    public async Task<ActionResult<ApiResponse<object>>> Upload(
        [FromForm] CustomStyleUploadRequestDto request,
        CancellationToken ct = default)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(ApiResponse<object>.Failure("משתמש לא מזוהה"));

        if (request.File is null || request.File.Length == 0)
            return BadRequest(ApiResponse<object>.Failure("לא נבחר קובץ"));

        var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return BadRequest(ApiResponse<object>.Failure("רק קבצי .inf או .info מותרים"));

        if (!IsAllowedMimeType(request.File.ContentType))
            return BadRequest(ApiResponse<object>.Failure("סוג קובץ לא תקין. רק קבצי INF/INFO מותרים"));

        var order = await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId.Value && o.PaymentStatus == PaymentStatus.Paid, ct);

        if (order is null)
            return BadRequest(ApiResponse<object>.Failure("הזמנה לא קיימת, לא שייכת למשתמש, או לא שולמה"));

        var safeName = Path.GetFileName(request.File.FileName);
        var blobPath = $"custom-style/user-uploads/{userId}/{request.OrderId}/{safeName}";

        await using var stream = request.File.OpenReadStream();
        var storedPath = await _blobStorage.UploadFileAsync(new BlobFileUploadRequest
        {
            BlobPath = blobPath,
            ContentType = string.IsNullOrWhiteSpace(request.File.ContentType) ? "application/octet-stream" : request.File.ContentType,
            Content = stream
        }, ct);

        var styleRequest = await _context.CustomStyleRequests
            .Include(r => r.Comments)
            .FirstOrDefaultAsync(r => r.OrderId == request.OrderId && r.UserId == userId.Value, ct);

        var now = DateTimeOffset.UtcNow;
        if (styleRequest is null)
        {
            styleRequest = new CustomStyleRequest
            {
                OrderId = request.OrderId,
                UserId = userId.Value,
                UserUploadUrl = storedPath,
                Status = CustomStyleRequestStatus.Pending,
                CreatedAt = now,
                UpdatedAt = now
            };
            _context.CustomStyleRequests.Add(styleRequest);
        }
        else
        {
            styleRequest.UserUploadUrl = storedPath;
            styleRequest.Status = CustomStyleRequestStatus.Pending;
            styleRequest.UpdatedAt = now;
        }

        await _context.SaveChangesAsync(ct);

        await _audit.WriteAsync(userId, "CUSTOM_STYLE_UPLOAD", "CustomStyleRequest", styleRequest.Id.ToString(), null, new { storedPath, request.OrderId }, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);

        return Ok(ApiResponse<object>.Success(new { id = styleRequest.Id, styleRequest.Status, styleRequest.UserUploadUrl }, "הקובץ הועלה והבקשה נוצרה בהצלחה"));
    }

    [HttpPost("{id:int}/comments")]
    public async Task<ActionResult<ApiResponse<object>>> AddComment(
        int id,
        [FromBody] AddCustomStyleCommentRequestDto request,
        CancellationToken ct = default)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(ApiResponse<object>.Failure("משתמש לא מזוהה"));

        var styleRequest = await _context.CustomStyleRequests
            .Include(r => r.Comments)
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId.Value, ct);

        if (styleRequest is null)
            return NotFound(ApiResponse<object>.Failure("בקשה לא נמצאה"));

        if (string.IsNullOrWhiteSpace(request.Content))
            return BadRequest(ApiResponse<object>.Failure("תוכן הערה לא יכול להיות ריק"));

        styleRequest.Comments.Add(new Comment
        {
            SenderName = GetUserDisplayName() ?? "Customer",
            Content = request.Content.Trim(),
            IsFromAdmin = false,
            CreatedAt = DateTimeOffset.UtcNow
        });

        styleRequest.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(ct);

        await _audit.WriteAsync(userId, "CUSTOM_STYLE_ADD_COMMENT", "CustomStyleRequest", styleRequest.Id.ToString(), null, new { request.Content }, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);

        return Ok(ApiResponse<object>.Success(new { id = styleRequest.Id }, "הערה נוספה בהצלחה"));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetById(int id, CancellationToken ct = default)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(ApiResponse<object>.Failure("משתמש לא מזוהה"));

        var styleRequest = await _context.CustomStyleRequests
            .AsNoTracking()
            .Where(r => r.Id == id && r.UserId == userId.Value)
            .Select(r => new CustomStyleRequestDto
            {
                Id = r.Id,
                OrderId = r.OrderId,
                UserId = r.UserId,
                Status = r.Status.ToString(),
                UserUploadUrl = r.UserUploadUrl,
                AdminProcessedUrl = r.AdminProcessedUrl,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                Comments = r.Comments
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CustomStyleCommentDto
                    {
                        Id = c.Id,
                        SenderName = c.SenderName,
                        Content = c.Content,
                        IsFromAdmin = c.IsFromAdmin,
                        CreatedAt = c.CreatedAt
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (styleRequest is null)
            return NotFound(ApiResponse<object>.Failure("בקשה לא נמצאה"));

        return Ok(ApiResponse<object>.Success(new { request = styleRequest }, "הבקשה נטענה בהצלחה"));
    }

    private Guid? GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var id) ? id : null;
    }

    private string? GetUserDisplayName()
    {
        var fullName = User.FindFirstValue(ClaimTypes.Name);
        if (!string.IsNullOrWhiteSpace(fullName))
            return fullName;

        var first = User.FindFirstValue(ClaimTypes.GivenName);
        var last = User.FindFirstValue(ClaimTypes.Surname);
        var name = $"{first} {last}".Trim();
        return string.IsNullOrWhiteSpace(name) ? null : name;
    }

    private static bool IsAllowedMimeType(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return false;

        return AllowedContentTypes.Contains(contentType.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
