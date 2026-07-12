using BeatsStoreYt.API.Data.Features.Commerce.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Cart;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.ProductType)
            .HasConversion<int>()
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_CartItems_ProductType_Enum", "[ProductType] IN (1, 2)"));

        builder.Property(i => i.UnitPriceSnapshot)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_CartItems_UnitPrice_NonNegative", "[UnitPriceSnapshot] >= 0"));

        builder.Property(i => i.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.ToTable(t => t.HasCheckConstraint("CK_CartItems_Quantity_Positive", "[Quantity] > 0"));

        builder.Property(i => i.AddedAt)
            .IsRequired();

        // One cart -> many cart items.
        builder.HasOne(i => i.ShoppingCart)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.ShoppingCartId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Prevent duplicate product rows inside one cart.
        builder.HasIndex(i => new { i.ShoppingCartId, i.ProductType, i.ProductId })
            .IsUnique();
    }
}
