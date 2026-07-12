namespace BeatsStoreYt.API.DTOs.Catalog;

public class BeatSetDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? DemoAudioUrl { get; set; }
    public int ItemsCount { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<BeatInSetDto> Items { get; set; } = new();
}

public class BeatInSetDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? DemoAudioUrl { get; set; }
    public string? ProductFileStorageKey { get; set; }
    public string? WaveformPeaks { get; set; }
    public string StyleName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
