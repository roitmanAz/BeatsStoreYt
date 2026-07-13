using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/admin/v1/reviews")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminReviewsController : ControllerBase
{
    private readonly BeatsStoreDbContext _context;
    private readonly IAuditLogService _audit;

    public AdminReviewsController(BeatsStoreDbContext context, IAuditLogService audit)
    {
        _context = context;
        _audit = audit;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAllReviews(
        [FromQuery] bool? isApproved,
        CancellationToken ct = default)
    {
        var query = _context.SiteReviews.AsNoTracking();
        if (isApproved.HasValue)
            query = query.Where(r => r.IsApproved == isApproved.Value);

        var reviews = await query
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new
            {
                r.Id,
                r.UserId,
                r.FirstName,
                r.LastName,
                r.Email,
                r.Content,
                r.Rating,
                r.IsApproved,
                r.CreatedAt,
                r.ApprovedAt
            })
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Success(new { reviews }, "ביקורות נטענו בהצלחה"));
    }

    [HttpPut("{id:int}/approve")]
    public async Task<ActionResult<ApiResponse<object>>> ApproveReview(int id, CancellationToken ct = default)
    {
        var review = await _context.SiteReviews.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (review is null)
            return NotFound(ApiResponse<object>.Failure("ביקורת לא נמצאה"));

        if (!review.IsApproved)
        {
            review.IsApproved = true;
            review.ApprovedAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync(ct);
        }

        await _audit.WriteAsync(
            GetAdminId(),
            "APPROVE_SITE_REVIEW",
            "SiteReview",
            review.Id.ToString(),
            null,
            new { review.IsApproved, review.ApprovedAt },
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);

        return Ok(ApiResponse<object>.Success(new { review.Id, review.IsApproved, review.ApprovedAt }, "הביקורת אושרה בהצלחה"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteReview(int id, CancellationToken ct = default)
    {
        var review = await _context.SiteReviews.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (review is null)
            return NotFound(ApiResponse<object>.Failure("ביקורת לא נמצאה"));

        _context.SiteReviews.Remove(review);
        await _context.SaveChangesAsync(ct);

        await _audit.WriteAsync(
            GetAdminId(),
            "DELETE_SITE_REVIEW",
            "SiteReview",
            id.ToString(),
            review,
            null,
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);

        return Ok(ApiResponse<object>.Success(new { id }, "הביקורת נמחקה בהצלחה"));
    }

    private Guid? GetAdminId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
