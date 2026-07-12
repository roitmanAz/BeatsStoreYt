using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.Data.Features.Commerce.Common;

namespace BeatsStoreYt.API.Data.Features.Commerce.Orders;

// Stores the order header for checkout, payment, totals, and status tracking.
// Used as the main record for purchase flow and downstream fulfillment.
public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal SubtotalAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal FinalAmount { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }

    public string Currency { get; set; } = "ILS";

    public string? TransactionId { get; set; }

    public string? DiscountCode { get; set; }

    public string? CustomerNotes { get; set; }

    public bool IsReceiptSent { get; set; }

    public bool IsInfoFileSent { get; set; }

    public DateTimeOffset OrderDate { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? PaidAt { get; set; }

    public DateTimeOffset? FailedAt { get; set; }

    // One user can place many orders over time.
    public User User { get; set; } = null!;

    // One order contains many order items.
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    // One order can have many payment attempts.
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
}
