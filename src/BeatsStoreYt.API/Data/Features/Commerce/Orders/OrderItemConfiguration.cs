using BeatsStoreYt.API.Data.Features.Commerce.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Orders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.ProductType)
            .HasConversion<int>()
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_OrderItems_ProductType_Enum", "[ProductType] IN (1, 2)"));

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_OrderItems_Quantity_Positive", "[Quantity] > 0"));

        builder.Property(i => i.PriceAtPurchase)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.LineDiscountAmount)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(i => i.LineTotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_OrderItems_Amounts_NonNegative", "[PriceAtPurchase] >= 0 AND [LineDiscountAmount] >= 0 AND [LineTotalAmount] >= 0"));

        // Many order items belong to one order.
        builder.HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(i => new { i.OrderId, i.ProductType, i.ProductId });
    }
}
