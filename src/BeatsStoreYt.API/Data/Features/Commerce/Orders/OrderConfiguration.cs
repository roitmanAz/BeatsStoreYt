using BeatsStoreYt.API.Data.Features.Commerce.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Orders;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.SubtotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.DiscountAmount)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(o => o.FinalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_Orders_Amounts_NonNegative", "[SubtotalAmount] >= 0 AND [DiscountAmount] >= 0 AND [FinalAmount] >= 0"));

        builder.Property(o => o.OrderStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(o => o.PaymentStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(o => o.PaymentMethod)
            .HasConversion<int>()
            .IsRequired(false);

        builder.Property(o => o.Currency)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("ILS");

        builder.Property(o => o.TransactionId)
            .HasMaxLength(150);

        builder.Property(o => o.DiscountCode)
            .HasMaxLength(50);

        builder.Property(o => o.CustomerNotes)
            .HasMaxLength(1000);

        builder.Property(o => o.IsReceiptSent)
            .IsRequired()
            .HasDefaultValue(false);

        // Tracks whether the manual info file was sent to the rhythm programmer.
        builder.Property(o => o.IsInfoFileSent)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(o => o.UpdatedAt)
            .IsRequired();

        builder.Property(o => o.PaidAt)
            .IsRequired(false);

        builder.Property(o => o.FailedAt)
            .IsRequired(false);

        // One user can place many orders over time.
        builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // One order contains many order items.
        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // One order can have many payment attempts.
        builder.HasMany(o => o.PaymentTransactions)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.UserId);
        builder.HasIndex(o => o.OrderDate);
        builder.HasIndex(o => o.OrderStatus);
        builder.HasIndex(o => o.PaymentStatus);
    }
}
