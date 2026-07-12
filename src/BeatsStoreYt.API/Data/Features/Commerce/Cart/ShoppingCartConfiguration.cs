using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Cart;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.ToTable("ShoppingCarts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.SessionId)
            .HasMaxLength(100);

        builder.Property(c => c.Currency)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("ILS");

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        // Optional user link supports guest carts.
        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Fast lookup for guest cart resume.
        builder.HasIndex(c => c.SessionId);

        builder.HasIndex(c => c.UserId);
    }
}
