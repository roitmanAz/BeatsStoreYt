using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Marketing;

public class EmailCampaignRecipientConfiguration : IEntityTypeConfiguration<EmailCampaignRecipient>
{
    public void Configure(EntityTypeBuilder<EmailCampaignRecipient> builder)
    {
        builder.ToTable("EmailCampaignRecipients");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Status)
            .IsRequired();

        builder.Property(r => r.SentAt)
            .IsRequired(false);

        builder.Property(r => r.OpenedAt)
            .IsRequired(false);

        builder.Property(r => r.ClickedAt)
            .IsRequired(false);

        builder.Property(r => r.FailureReason)
            .HasMaxLength(500);

        // Many recipient rows belong to one campaign.
        builder.HasOne(r => r.Campaign)
            .WithMany(c => c.Recipients)
            .HasForeignKey(r => r.CampaignId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Many recipient rows point to one subscription.
        builder.HasOne(r => r.Subscription)
            .WithMany()
            .HasForeignKey(r => r.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(r => new { r.CampaignId, r.SubscriptionId })
            .IsUnique();
    }
}
