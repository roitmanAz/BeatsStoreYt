using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Events;

// Stores the customer event request form before any phone confirmation happens.
// Used as the main intake record for weddings and full-event inquiries.
public class EventRequest
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string EventType { get; set; } = string.Empty;

    public DateOnly? EventDate { get; set; }

    public string? City { get; set; }

    public string? Notes { get; set; }

    public int Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // Optional user link allows logged-in customers to submit event forms.
    public User? User { get; set; }

    // One event request can later have many phone calls or follow-ups.
    public ICollection<EventRequestCall> Calls { get; set; } = new List<EventRequestCall>();

    // One event request can later have one summary record.
    public EventSummary? Summary { get; set; }
}
