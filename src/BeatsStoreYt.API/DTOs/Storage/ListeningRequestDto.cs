namespace BeatsStoreYt.API.DTOs.Storage;

public class ListeningRequestDto
{
    public string BlobPath { get; set; } = string.Empty;

    public int ValidMinutes { get; set; } = 10;
}
