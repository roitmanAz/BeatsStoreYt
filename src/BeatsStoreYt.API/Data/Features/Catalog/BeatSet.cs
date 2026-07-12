namespace BeatsStoreYt.API.Data.Features.Catalog;

// Stores complete beat bundles (sets) that can be offered as grouped products.
// Used for package pricing, promotions, and curated collections of rhythms.
public class BeatSet
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? CoverImageUrl { get; set; }

    public string? DemoAudioUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One beat set contains many beat-set link rows.
    public ICollection<BeatSetItem> BeatSetItems { get; set; } = new List<BeatSetItem>();
}
