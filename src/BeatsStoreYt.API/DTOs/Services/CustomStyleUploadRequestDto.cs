namespace BeatsStoreYt.API.DTOs.Services;

public class CustomStyleUploadRequestDto
{
    public Guid OrderId { get; set; }

    public IFormFile File { get; set; } = null!;
}
