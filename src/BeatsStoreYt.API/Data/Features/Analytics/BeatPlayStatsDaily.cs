using BeatsStoreYt.API.Data.Features.Catalog;
using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Analytics;

// Stores aggregated daily beat play metrics for fast dashboard reporting.
// Used to avoid scanning raw play events for common admin statistics.
public class BeatPlayStatsDaily
{
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public int BeatId { get; set; }

    public Guid? UserId { get; set; }

    public int PlayCount { get; set; }

    public int UniqueListeners { get; set; }

    public int TotalPlayedSeconds { get; set; }

    // Many aggregated rows belong to one beat.
    public Beat Beat { get; set; } = null!;

    // Optional user breakdown for per-customer reporting.
    public User? User { get; set; }
}
