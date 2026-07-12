using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Commerce.Favorites;

// Stores a user's persistent wishlist independent from checkout flow.
// Used to let users bookmark products for later decisions.
public class Favorite
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One user has one favorites container.
    // This keeps wishlist data separate from cart data and purchase flow.
    public User User { get; set; } = null!;

    // One favorites container holds many favorite items.
    // Each item represents one bookmarked product.
    public ICollection<FavoriteItem> Items { get; set; } = new List<FavoriteItem>();
}
