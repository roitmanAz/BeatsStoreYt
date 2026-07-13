using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Content;
using BeatsStoreYt.API.DTOs.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly BeatsStoreDbContext _context;

    public ReviewsController(BeatsStoreDbContext context)
    {
        _context = context;
    }

    [HttpPost("site")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<object>>> CreateSiteReview(
        [FromBody] CreateSiteReviewRequestDto request,
        CancellationToken ct = default)
    {
        var review = new SiteReview
        {
            FirstName = SanitizeText(request.FirstName),
            LastName = SanitizeText(request.LastName),
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            Content = SanitizeText(request.Content),
            Rating = request.Rating,
            UserId = GetUserId(),
            CreatedAt = DateTimeOffset.UtcNow,
            IsApproved = false,
            ApprovedAt = null
        };

        _context.SiteReviews.Add(review);
        await _context.SaveChangesAsync(ct);

        return Ok(ApiResponse<object>.Success(new { reviewId = review.Id, isApproved = review.IsApproved }, "הביקורת התקבלה וממתינה לאישור"));
    }

    [HttpGet("site")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PagedResult<SiteReviewDto>>>> GetApprovedSiteReviews(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _context.SiteReviews
            .AsNoTracking()
            .Where(r => r.IsApproved);

        var totalItems = await query.CountAsync(ct);

        var reviews = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new SiteReviewDto
            {
                Id = r.Id,
                CreatedAt = r.CreatedAt,
                FirstName = r.FirstName,
                LastName = r.LastName,
                Content = r.Content,
                Rating = r.Rating
            })
            .ToListAsync(ct);

        var result = PagedResult<SiteReviewDto>.Create(reviews, page, pageSize, totalItems);
        return Ok(ApiResponse<PagedResult<SiteReviewDto>>.Success(result, "ביקורות מאושרות נטענו בהצלחה"));
    }

    [HttpPost("/api/v1/events/{eventId:guid}/comments")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<object>>> CreateEventComment(
        Guid eventId,
        [FromBody] CreateEventCommentRequestDto request,
        CancellationToken ct = default)
    {
        var eventExists = await _context.WeddingShowcases
            .AsNoTracking()
            .AnyAsync(e => e.Id == eventId, ct);

        if (!eventExists)
            return NotFound(ApiResponse<object>.Failure("אירוע לא נמצא"));

        var comment = new EventComment
        {
            WeddingShowcaseId = eventId,
            FirstName = SanitizeText(request.FirstName),
            LastName = SanitizeText(request.LastName),
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            Content = SanitizeText(request.Content),
            UserId = GetUserId(),
            CreatedAt = DateTimeOffset.UtcNow,
            IsApproved = false,
            ApprovedAt = null
        };

        _context.EventComments.Add(comment);
        await _context.SaveChangesAsync(ct);

        return Ok(ApiResponse<object>.Success(new { commentId = comment.Id, isApproved = comment.IsApproved }, "התגובה התקבלה וממתינה לאישור"));
    }

    [HttpGet("/api/v1/events/{eventId:guid}/comments")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<object>>> GetApprovedEventComments(
        Guid eventId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _context.EventComments
            .AsNoTracking()
            .Where(c => c.WeddingShowcaseId == eventId && c.IsApproved);

        var totalItems = await query.CountAsync(ct);

        var comments = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new EventCommentDto
            {
                Id = c.Id,
                CreatedAt = c.CreatedAt,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Content = c.Content
            })
            .ToListAsync(ct);

        var result = PagedResult<EventCommentDto>.Create(comments, page, pageSize, totalItems);
        return Ok(ApiResponse<PagedResult<EventCommentDto>>.Success(result, "תגובות מאושרות נטענו בהצלחה"));
    }

    private Guid? GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var id) ? id : null;
    }

    private static string SanitizeText(string value)
    {
        var noTags = Regex.Replace(value, "<.*?>", string.Empty);
        return WebUtility.HtmlEncode(noTags.Trim());
    }
}
