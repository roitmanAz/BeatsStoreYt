namespace BeatsStoreYt.API.Data.Features.Commerce.Cart;

// Stores line items inside a cart with price snapshot and quantity.
// Used to calculate totals and later convert cart content into order lines.
public class CartItem
{
    public Guid Id { get; set; }

    public Guid ShoppingCartId { get; set; }

    public Common.CatalogProductType ProductType { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPriceSnapshot { get; set; }

    public int Quantity { get; set; } = 1;

    public DateTimeOffset AddedAt { get; set; }

    // Many cart items belong to one shopping cart.
    // This is the core one-to-many relationship that makes the cart reusable.
    public ShoppingCart ShoppingCart { get; set; } = null!;
}
