using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Orders;

public class PaymentWebhookConfiguration : IEntityTypeConfiguration<PaymentWebhook>
{
    public void Configure(EntityTypeBuilder<PaymentWebhook> builder)
    {
        builder.ToTable("PaymentWebhooks");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.ProviderName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(w => w.EventType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.ProviderEventId)
            .HasMaxLength(150);

        builder.Property(w => w.PayloadJson)
            .IsRequired();

        builder.Property(w => w.ReceivedAt)
            .IsRequired();

        builder.Property(w => w.ProcessedAt)
            .IsRequired(false);

        builder.Property(w => w.IsProcessed)
            .IsRequired()
            .HasDefaultValue(false);

        // Optional order link keeps webhook storage flexible for orphan events.
        builder.HasOne(w => w.Order)
            .WithMany()
            .HasForeignKey(w => w.OrderId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(w => w.ProviderEventId);
        builder.HasIndex(w => new { w.ProviderName, w.EventType });
        builder.HasIndex(w => w.IsProcessed);
    }
}
