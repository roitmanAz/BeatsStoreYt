using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Marketing;

// Stores newsletter opt-in data for users and guests.
// Used to manage mailing list subscriptions, consent, and contact channels.
public class NewsletterSubscription
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public int Channel { get; set; }

    public bool IsSubscribed { get; set; }

    public DateTimeOffset? SubscribedAt { get; set; }

    public DateTimeOffset? UnsubscribedAt { get; set; }

    public string? Source { get; set; }

    public string? ConsentTextVersion { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // Optional link allows syncing a guest subscription with a registered account.
    public User? User { get; set; }
}
