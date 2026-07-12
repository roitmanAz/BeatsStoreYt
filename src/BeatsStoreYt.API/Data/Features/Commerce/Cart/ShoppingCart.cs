using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Commerce.Cart;

// Stores one active cart per user/session to keep pre-checkout selections.
// Supports both authenticated users and guests through SessionId.
public class ShoppingCart
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    // Guest cart identifier from cookie/session storage.
    public string? SessionId { get; set; }

    public string Currency { get; set; } = "ILS";

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One user can own multiple carts over time.
    // Guests can also resume the same cart using SessionId.
    public User? User { get; set; }

    // One cart contains many cart items.
    // Items are stored separately so totals and snapshots can be calculated safely.
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
