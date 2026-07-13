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
[Route("api/admin/v1/event-comments")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminEventCommentsController : ControllerBase
{
    private readonly BeatsStoreDbContext _context;
    private readonly IAuditLogService _audit;

    public AdminEventCommentsController(BeatsStoreDbContext context, IAuditLogService audit)
    {
        _context = context;
        _audit = audit;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAllEventComments(
        [FromQuery] bool? isApproved,
        CancellationToken ct = default)
    {
        var query = _context.EventComments
            .AsNoTracking()
            .Include(c => c.WeddingShowcase)
            .AsQueryable();

        if (isApproved.HasValue)
            query = query.Where(c => c.IsApproved == isApproved.Value);

        var comments = await query
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new
            {
                c.Id,
                c.WeddingShowcaseId,
                eventTitle = c.WeddingShowcase.Title,
                c.UserId,
                c.FirstName,
                c.LastName,
                c.Email,
                c.Content,
                c.IsApproved,
                c.CreatedAt,
                c.ApprovedAt
            })
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Success(new { comments }, "תגובות אירוע נטענו בהצלחה"));
    }

    [HttpPut("{id:int}/approve")]
    public async Task<ActionResult<ApiResponse<object>>> ApproveEventComment(int id, CancellationToken ct = default)
    {
        var comment = await _context.EventComments.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (comment is null)
            return NotFound(ApiResponse<object>.Failure("תגובה לא נמצאה"));

        if (!comment.IsApproved)
        {
            comment.IsApproved = true;
            comment.ApprovedAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync(ct);
        }

        await _audit.WriteAsync(
            GetAdminId(),
            "APPROVE_EVENT_COMMENT",
            "EventComment",
            comment.Id.ToString(),
            null,
            new { comment.IsApproved, comment.ApprovedAt },
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);

        return Ok(ApiResponse<object>.Success(new { comment.Id, comment.IsApproved, comment.ApprovedAt }, "התגובה אושרה בהצלחה"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteEventComment(int id, CancellationToken ct = default)
    {
        var comment = await _context.EventComments.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (comment is null)
            return NotFound(ApiResponse<object>.Failure("תגובה לא נמצאה"));

        _context.EventComments.Remove(comment);
        await _context.SaveChangesAsync(ct);

        await _audit.WriteAsync(
            GetAdminId(),
            "DELETE_EVENT_COMMENT",
            "EventComment",
            id.ToString(),
            comment,
            null,
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);

        return Ok(ApiResponse<object>.Success(new { id }, "התגובה נמחקה בהצלחה"));
    }

    private Guid? GetAdminId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
