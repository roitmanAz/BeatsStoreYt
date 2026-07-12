using System.Text.Json;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Security;

namespace BeatsStoreYt.API.Services.Auth;

public class SecurityEventLogger : ISecurityEventLogger
{
    private readonly BeatsStoreDbContext _context;

    public SecurityEventLogger(BeatsStoreDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(Guid? userId, string eventType, int severity, object? details = null, CancellationToken ct = default)
    {
        _context.SecurityEvents.Add(new SecurityEvent
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            EventType = eventType,
            Severity = severity,
            DetailsJson = details is null ? null : JsonSerializer.Serialize(details),
            CreatedAt = DateTimeOffset.UtcNow
        });

        await _context.SaveChangesAsync(ct);
    }
}
