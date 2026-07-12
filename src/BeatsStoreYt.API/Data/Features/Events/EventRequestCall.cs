using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Events;

// Stores phone call follow-up records for each event request.
// Used by the admin to document confirmation calls and their outcomes.
public class EventRequestCall
{
    public Guid Id { get; set; }

    public Guid EventRequestId { get; set; }

    public Guid? AdminUserId { get; set; }

    public DateTimeOffset CallAt { get; set; }

    public int CallResult { get; set; }

    public string? Summary { get; set; }

    // Many calls belong to one event request.
    public EventRequest EventRequest { get; set; } = null!;

    // Optional admin link allows tracking which staff member made the call.
    public User? AdminUser { get; set; }
}
