namespace BeatsStoreYt.API.DTOs.Admin;

public class AdminUpdateBeatSetDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? DemoAudioUrl { get; set; }
    public bool IsActive { get; set; }
}
