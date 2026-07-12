namespace BeatsStoreYt.API.Data.Features.Catalog;

// Stores individual rhythm products with pricing, media previews, and file references.
// Used as the main sellable/listenable catalog item across storefront features.
public class Beat
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int KeyboardModelId { get; set; }

    // Many beats belong to one keyboard model.
    public KeyboardModel KeyboardModel { get; set; } = null!;

    public int StyleId { get; set; }

    // Many beats belong to one style.
    public Style Style { get; set; } = null!;

    public string? CoverImageUrl { get; set; }

    public string DemoAudioUrl { get; set; } = string.Empty;

    public string? ProductFileStorageKey { get; set; }

    public string? WaveformPeaks { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One beat can appear in many beat sets through link rows.
    public ICollection<BeatSetItem> BeatSetItems { get; set; } = new List<BeatSetItem>();
}
