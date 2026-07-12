namespace BeatsStoreYt.API.Data.Features.Marketing;

// Stores one recipient row for a marketing campaign.
// Used to track queueing, sending, opens, clicks, and failures per recipient.
public class EmailCampaignRecipient
{
    public Guid Id { get; set; }

    public Guid CampaignId { get; set; }

    public Guid SubscriptionId { get; set; }

    public int Status { get; set; }

    public DateTimeOffset? SentAt { get; set; }

    public DateTimeOffset? OpenedAt { get; set; }

    public DateTimeOffset? ClickedAt { get; set; }

    public string? FailureReason { get; set; }

    // Many recipient rows belong to one campaign.
    public EmailCampaign Campaign { get; set; } = null!;

    // Many recipient rows point to one subscription.
    public NewsletterSubscription Subscription { get; set; } = null!;
}
