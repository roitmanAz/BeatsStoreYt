namespace BeatsStoreYt.API.DTOs.Catalog;

public class BeatDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? DemoAudioUrl { get; set; }
    public bool IsActive { get; set; }
    public StyleDto Style { get; set; } = new();
    public KeyboardModelDto KeyboardModel { get; set; } = new();
    public List<BeatSetItemDto> BeatSets { get; set; } = new();
}

public class BeatSetItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
