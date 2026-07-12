namespace BeatsStoreYt.API.DTOs.Catalog;

public class BeatSetDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }    public string? CoverImageUrl { get; set; }
    public string? DemoAudioUrl { get; set; }    public List<BeatInSetDto> Items { get; set; } = new();
}

public class BeatInSetDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? DemoAudioUrl { get; set; }
    public string StyleName { get; set; } = string.Empty;
}
