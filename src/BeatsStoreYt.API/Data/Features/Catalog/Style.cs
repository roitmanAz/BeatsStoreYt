namespace BeatsStoreYt.API.Data.Features.Catalog;

// Stores music styles used to classify beats in a dynamic admin-managed catalog.
// Used for filtering, organizing, and assigning beats to musical style categories.
public class Style
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One style can be assigned to many beats.
    public ICollection<Beat> Beats { get; set; } = new List<Beat>();
}
