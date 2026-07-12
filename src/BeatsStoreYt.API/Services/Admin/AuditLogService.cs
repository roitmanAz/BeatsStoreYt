using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Security;

namespace BeatsStoreYt.API.Services.Admin;

public class AuditLogService : IAuditLogService
{
    private readonly BeatsStoreDbContext _context;

    public AuditLogService(BeatsStoreDbContext context)
    {
        _context = context;
    }

    public async Task WriteAsync(
        Guid? userId,
        string action,
        string entityName,
        string entityId,
        object? oldValues,
        object? newValues,
        string? ip,
        CancellationToken ct = default)
    {
        _context.AuditLogs.Add(new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            OldValuesJson = oldValues is null ? null : JsonSerializer.Serialize(oldValues),
            NewValuesJson = newValues is null ? null : JsonSerializer.Serialize(newValues),
            IpHash = HashIp(ip),
            CreatedAt = DateTimeOffset.UtcNow
        });

        await _context.SaveChangesAsync(ct);
    }

    private static string? HashIp(string? ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
            return null;

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(ip));
        return Convert.ToHexString(bytes);
    }
}
