using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/admin/v1/event-comments")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminEventCommentsController : BaseAdminController
{
    private readonly BeatsStoreDbContext _context;

    public AdminEventCommentsController(BeatsStoreDbContext context, IAuditLogService audit)
        : base(audit)
    {
        _context = context;
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
        var setStatusResult = await SetBooleanStatusAsync(
            comment,
            "תגובה לא נמצאה",
            c => c.IsApproved,
            (c, status) =>
            {
                c.IsApproved = status;
                c.ApprovedAt = status ? DateTimeOffset.UtcNow : null;
            },
            true,
            _context,
            "APPROVE_EVENT_COMMENT",
            "EventComment",
            id.ToString(),
            c => new { c.IsApproved, c.ApprovedAt },
            ct);

        if (setStatusResult is not null)
            return setStatusResult;

        return Ok(ApiResponse<object>.Success(new { comment!.Id, comment.IsApproved, comment.ApprovedAt }, "התגובה אושרה בהצלחה"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteEventComment(int id, CancellationToken ct = default)
    {
        var comment = await _context.EventComments.FirstOrDefaultAsync(c => c.Id == id, ct);
        var deleteResult = await DeleteEntityAsync(
            _context.EventComments,
            comment,
            "תגובה לא נמצאה",
            "DELETE_EVENT_COMMENT",
            "EventComment",
            id.ToString(),
            _context,
            ct);

        if (deleteResult is not null)
            return deleteResult;

        return Ok(ApiResponse<object>.Success(new { id }, "התגובה נמחקה בהצלחה"));
    }
}
