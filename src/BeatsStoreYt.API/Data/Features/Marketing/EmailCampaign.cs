namespace BeatsStoreYt.API.Data.Features.Marketing;

// Stores a planned or sent email marketing campaign.
// Used to manage newsletter sends, automation, and campaign delivery tracking.
public class EmailCampaign
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string TemplateKey { get; set; } = string.Empty;

    public int Status { get; set; }

    public DateTimeOffset? ScheduledAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One campaign can target many recipients.
    public ICollection<EmailCampaignRecipient> Recipients { get; set; } = new List<EmailCampaignRecipient>();
}
