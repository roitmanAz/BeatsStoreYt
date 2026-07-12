namespace BeatsStoreYt.API.DTOs.Admin;

public class AdminUpdateBeatDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int KeyboardModelId { get; set; }
    public int StyleId { get; set; }
    public string? CoverImageUrl { get; set; }
    public string DemoAudioUrl { get; set; } = string.Empty;
    public string? ProductFileStorageKey { get; set; }
    public string? WaveformPeaks { get; set; }
    public bool IsActive { get; set; }
}
