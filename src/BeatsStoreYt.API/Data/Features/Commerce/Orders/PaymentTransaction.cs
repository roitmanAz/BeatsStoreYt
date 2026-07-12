using BeatsStoreYt.API.Data.Features.Commerce.Common;

namespace BeatsStoreYt.API.Data.Features.Commerce.Orders;

// Stores each payment attempt separately for debugging, auditing, and retries.
// Used to track provider responses and transaction outcomes over time.
public class PaymentTransaction
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string ProviderName { get; set; } = string.Empty;

    public string? ProviderTransactionId { get; set; }

    public int AttemptNumber { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "ILS";

    public PaymentStatus Status { get; set; }

    public string? ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    // Many payment transactions belong to one order.
    public Order Order { get; set; } = null!;
}
