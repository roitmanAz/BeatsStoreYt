using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Security;

// Stores important admin and system changes for auditing.
// Used to trace who changed what, when, and on which entity.
public class AuditLog
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public string EntityId { get; set; } = string.Empty;

    public string? OldValuesJson { get; set; }

    public string? NewValuesJson { get; set; }

    public string? IpHash { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    // Optional user link helps identify which account made the change.
    public User? User { get; set; }
}
