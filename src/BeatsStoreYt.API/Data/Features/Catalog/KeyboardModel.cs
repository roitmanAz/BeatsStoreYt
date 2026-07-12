namespace BeatsStoreYt.API.Data.Features.Catalog;

// Stores keyboard models and brands so beats can target specific instruments.
// Used to ensure compatibility and improve catalog filtering by brand/model.
public class KeyboardModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public KeyboardBrand Brand { get; set; } = KeyboardBrand.Yamaha;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One keyboard model can be linked to many beats.
    public ICollection<Beat> Beats { get; set; } = new List<Beat>();
}
