using BeatsStoreYt.API.Data.Features.Commerce.Common;

namespace BeatsStoreYt.API.Data.Features.Commerce.Orders;

// Stores the purchased product snapshot so later price changes do not affect history.
// Used to preserve exactly what the customer bought inside a specific order.
public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public CatalogProductType ProductType { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal PriceAtPurchase { get; set; }

    public decimal LineDiscountAmount { get; set; }

    public decimal LineTotalAmount { get; set; }

    // Many order items belong to one order.
    public Order Order { get; set; } = null!;
}
