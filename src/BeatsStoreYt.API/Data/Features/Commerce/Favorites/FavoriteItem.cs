namespace BeatsStoreYt.API.Data.Features.Commerce.Favorites;

// Stores each product bookmarked by the user in the favorites list.
// Keeps favorites normalized and prevents duplicate entries per product.
public class FavoriteItem
{
    public Guid Id { get; set; }

    public Guid FavoriteId { get; set; }

    public Common.CatalogProductType ProductType { get; set; }

    public int ProductId { get; set; }

    public DateTimeOffset AddedAt { get; set; }

    // Many favorite items belong to one favorites container.
    // This makes the wishlist expandable without duplicating the list record.
    public Favorite Favorite { get; set; } = null!;
}
