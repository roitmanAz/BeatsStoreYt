using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Support;

// Stores the main support conversation request opened by a customer or admin.
// Used for help requests, status tracking, and customer service workflow.
public class SupportTicket
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public int Status { get; set; }

    public int Priority { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? ClosedAt { get; set; }

    // One user can open many support tickets.
    public User User { get; set; } = null!;

    // One support ticket contains many messages.
    public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
}
