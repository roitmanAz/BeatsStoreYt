using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Orders;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("PaymentTransactions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ProviderName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.ProviderTransactionId)
            .HasMaxLength(150);

        builder.Property(p => p.AttemptNumber)
            .IsRequired();

        builder.Property(p => p.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Currency)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("ILS");

        builder.Property(p => p.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(p => p.ErrorCode)
            .HasMaxLength(100);

        builder.Property(p => p.ErrorMessage)
            .HasMaxLength(500);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // Many payment transactions belong to one order.
        builder.HasOne(p => p.Order)
            .WithMany(o => o.PaymentTransactions)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(p => new { p.OrderId, p.AttemptNumber })
            .IsUnique();
    }
}
