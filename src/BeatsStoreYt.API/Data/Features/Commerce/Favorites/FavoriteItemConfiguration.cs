using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Favorites;

public class FavoriteItemConfiguration : IEntityTypeConfiguration<FavoriteItem>
{
    public void Configure(EntityTypeBuilder<FavoriteItem> builder)
    {
        builder.ToTable("FavoriteItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.ProductType)
            .HasConversion<int>()
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_FavoriteItems_ProductType_Enum", "[ProductType] IN (1, 2)"));

        builder.Property(i => i.AddedAt)
            .IsRequired();

        // One favorites list -> many favorite items.
        // This lets a user bookmark multiple products in a single wishlist.
        builder.HasOne(i => i.Favorite)
            .WithMany(f => f.Items)
            .HasForeignKey(i => i.FavoriteId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Prevent duplicate favorites for the same product in one list.
        builder.HasIndex(i => new { i.FavoriteId, i.ProductType, i.ProductId })
            .IsUnique();
    }
}
