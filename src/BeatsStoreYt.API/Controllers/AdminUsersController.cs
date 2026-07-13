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
[Route("api/admin/v1/users")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminUsersController : ControllerBase
{
    private readonly BeatsStoreDbContext _context;
    private readonly IAuditLogService _audit;

    public AdminUsersController(BeatsStoreDbContext context, IAuditLogService audit)
    {
        _context = context;
        _audit = audit;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAllUsers(CancellationToken ct = default)
    {
        await _audit.WriteAsync(
            GetAdminId(),
            "VIEW_USERS_LIST",
            "User",
            "All",
            null,
            null,
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);

        var users = await _context.Users
            .AsNoTracking()
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.PhoneNumber,
                u.Role,
                u.CreatedAt,
                u.UpdatedAt,
                u.LastLoginAt,
                u.IsActive,
                u.EmailConfirmed,
                u.DeletedAt,
                u.PasswordResetTokenCreatedAt
            })
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Success(new { users }, "משתמשים נטענו בהצלחה"));
    }

    private Guid? GetAdminId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
