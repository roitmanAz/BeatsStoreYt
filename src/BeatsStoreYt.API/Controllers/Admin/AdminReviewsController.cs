using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/admin/v1/reviews")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminReviewsController : BaseAdminController
{
    private readonly BeatsStoreDbContext _context;

    public AdminReviewsController(BeatsStoreDbContext context)
    {
        _context = context;
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
        var setStatusResult = await SetBooleanStatusAsync(
            review,
            "ביקורת לא נמצאה",
            r => r.IsApproved,
            (r, status) =>
            {
                r.IsApproved = status;
                r.ApprovedAt = status ? DateTimeOffset.UtcNow : null;
            },
            true,
            _context,
            "APPROVE_SITE_REVIEW",
            "SiteReview",
            id.ToString(),
            r => new { r.IsApproved, r.ApprovedAt },
            ct);

        if (setStatusResult is not null)
            return setStatusResult;

        return Ok(ApiResponse<object>.Success(new { review!.Id, review.IsApproved, review.ApprovedAt }, "הביקורת אושרה בהצלחה"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteReview(int id, CancellationToken ct = default)
    {
        var review = await _context.SiteReviews.FirstOrDefaultAsync(r => r.Id == id, ct);
        var deleteResult = await DeleteEntityAsync(
            _context.SiteReviews,
            review,
            "ביקורת לא נמצאה",
            "DELETE_SITE_REVIEW",
            "SiteReview",
            id.ToString(),
            _context,
            ct);

        if (deleteResult is not null)
            return deleteResult;

        return Ok(ApiResponse<object>.Success(new { id }, "הביקורת נמחקה בהצלחה"));
    }
}
