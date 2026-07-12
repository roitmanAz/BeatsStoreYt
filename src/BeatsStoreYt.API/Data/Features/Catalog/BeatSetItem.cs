namespace BeatsStoreYt.API.Data.Features.Catalog;

// Join table linking beats to beat sets with optional ordering inside each set.
// Used to model many-to-many relationships between bundles and rhythm items.
public class BeatSetItem
{
    public int Id { get; set; }

    public int BeatSetId { get; set; }

    // Many link rows belong to one beat set.
    public BeatSet BeatSet { get; set; } = null!;

    public int BeatId { get; set; }

    // Many link rows can point to one beat.
    public Beat Beat { get; set; } = null!;

    public int SortOrder { get; set; }
}
