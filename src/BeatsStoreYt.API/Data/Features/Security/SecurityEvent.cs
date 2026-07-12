using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Security;

// Stores security-related incidents such as suspicious downloads or failed access attempts.
// Used to monitor abuse, investigate incidents, and enforce safer access rules.
public class SecurityEvent
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string EventType { get; set; } = string.Empty;

    public int Severity { get; set; }

    public string? DetailsJson { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    // Optional user link helps identify the account involved in the event.
    public User? User { get; set; }
}
