namespace BeatsStoreYt.API.Data.Features.Commerce.Orders;

// Stores raw payment provider callbacks to keep external payment events auditable.
// Used to process webhook notifications safely even if the browser flow is interrupted.
public class PaymentWebhook
{
    public Guid Id { get; set; }

    public string ProviderName { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;

    public string? ProviderEventId { get; set; }

    public Guid? OrderId { get; set; }

    public string PayloadJson { get; set; } = string.Empty;

    public DateTimeOffset ReceivedAt { get; set; }

    public DateTimeOffset? ProcessedAt { get; set; }

    public bool IsProcessed { get; set; }

    // Optional link to the order if the webhook belongs to a checkout flow.
    public Order? Order { get; set; }
}
