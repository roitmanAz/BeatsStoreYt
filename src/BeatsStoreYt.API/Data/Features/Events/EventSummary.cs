using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Events;

// Stores the final summary of an event request after admin review and phone confirmation.
// Used to keep the approved outcome, notes, and agreed terms in one place.
public class EventSummary
{
    public Guid Id { get; set; }

    public Guid EventRequestId { get; set; }

    public DateTimeOffset? ConfirmedAt { get; set; }

    public Guid? ConfirmedByAdminId { get; set; }

    public int FinalStatus { get; set; }

    public decimal? AgreedPrice { get; set; }

    public string? SummaryNotes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One summary belongs to one event request.
    public EventRequest EventRequest { get; set; } = null!;

    // Optional admin link records who finalized the request.
    public User? ConfirmedByAdmin { get; set; }
}
