namespace BeatsStoreYt.API.Data.Features.Content;

// Stores a wedding or full-event showcase entry for marketing and content pages.
// Used to present complete event media while pointing to Blob Storage assets.
public class WeddingShowcase
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid MediaAssetId { get; set; }

    public DateOnly? EventDate { get; set; }

    public bool IsFeatured { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One showcase points to one stored media asset.
    // This is where the full wedding or event preview can be displayed from Blob Storage.
    public MediaAsset MediaAsset { get; set; } = null!;

    public ICollection<EventComment> EventComments { get; set; } = new List<EventComment>();
}
