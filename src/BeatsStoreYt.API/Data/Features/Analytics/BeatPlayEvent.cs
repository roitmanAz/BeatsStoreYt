using BeatsStoreYt.API.Data.Features.Catalog;
using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Analytics;

// Stores every beat play event for analytics and usage tracking.
// Used to measure listen counts, duration, and user-level engagement.
public class BeatPlayEvent
{
    public Guid Id { get; set; }

    public int BeatId { get; set; }

    public Guid? UserId { get; set; }

    public string? SessionId { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public int PlayedSeconds { get; set; }

    public decimal CompletedPercent { get; set; }

    public string? Source { get; set; }

    public string? IpHash { get; set; }

    // Many play events belong to one beat.
    public Beat Beat { get; set; } = null!;

    // Optional user link allows tracking engagement per customer.
    public User? User { get; set; }
}
