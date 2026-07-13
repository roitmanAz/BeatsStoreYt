using BeatsStoreYt.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeatsStoreYt.API.Middleware;

public class UserActivityMiddleware
{
    private static readonly TimeSpan UpdateInterval = TimeSpan.FromHours(1);

    private readonly RequestDelegate _next;

    public UserActivityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, BeatsStoreDbContext dbContext)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var userIdValue = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? context.User.FindFirstValue("sub");

            if (Guid.TryParse(userIdValue, out var userId))
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, context.RequestAborted);
                if (user is not null)
                {
                    var now = DateTimeOffset.UtcNow;
                    if (!user.LastActiveAt.HasValue || now - user.LastActiveAt.Value >= UpdateInterval)
                    {
                        user.LastActiveAt = now;
                        user.UpdatedAt = now;
                        await dbContext.SaveChangesAsync(context.RequestAborted);
                    }
                }
            }
        }

        await _next(context);
    }
}
