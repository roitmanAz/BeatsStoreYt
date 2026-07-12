using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Support;

// Stores each message inside a support ticket conversation.
// Used to preserve the chat history between the customer and the admin team.
public class TicketMessage
{
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }

    public Guid SenderId { get; set; }

    public string MessageBody { get; set; } = string.Empty;

    public DateTimeOffset SentAt { get; set; }

    public bool IsRead { get; set; }

    // Many messages belong to one support ticket.
    public SupportTicket Ticket { get; set; } = null!;

    // The sender can be either the customer or an admin user.
    public User Sender { get; set; } = null!;
}
