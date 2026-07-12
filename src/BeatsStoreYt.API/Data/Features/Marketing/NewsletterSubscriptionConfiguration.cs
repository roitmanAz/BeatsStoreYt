using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Marketing;

public class NewsletterSubscriptionConfiguration : IEntityTypeConfiguration<NewsletterSubscription>
{
    public void Configure(EntityTypeBuilder<NewsletterSubscription> builder)
    {
        builder.ToTable("NewsletterSubscriptions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(s => s.Channel)
            .IsRequired();

        builder.Property(s => s.IsSubscribed)
            .IsRequired();

        builder.Property(s => s.SubscribedAt)
            .IsRequired(false);

        builder.Property(s => s.UnsubscribedAt)
            .IsRequired(false);

        builder.Property(s => s.Source)
            .HasMaxLength(100);

        builder.Property(s => s.ConsentTextVersion)
            .HasMaxLength(50);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        // Optional user link allows syncing a guest subscription with a registered account.
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(s => s.Email);
        builder.HasIndex(s => s.Channel);
        builder.HasIndex(s => s.IsSubscribed);
    }
}
