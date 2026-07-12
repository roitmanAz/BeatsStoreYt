namespace BeatsStoreYt.API.DTOs.Catalog;

public class BeatListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? DemoAudioUrl { get; set; }
    public string StyleName { get; set; } = string.Empty;
    public string KeyboardModelName { get; set; } = string.Empty;
    public string KeyboardBrand { get; set; } = string.Empty;
}
