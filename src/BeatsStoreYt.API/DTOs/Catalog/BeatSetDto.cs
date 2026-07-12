namespace BeatsStoreYt.API.DTOs.Catalog;

public class BeatSetDto
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
}
