using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/admin/v1/users")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminUsersController : BaseAdminController
{
    private readonly BeatsStoreDbContext _context;

    public AdminUsersController(BeatsStoreDbContext context, IAuditLogService audit)
        : base(audit)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAllUsers(CancellationToken ct = default)
    {
        await WriteAdminAuditAsync("VIEW_USERS_LIST", "User", "All", null, null, ct);

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

}
