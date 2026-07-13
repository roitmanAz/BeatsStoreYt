namespace BeatsStoreYt.API.DTOs.Services;

public class CustomStyleRequestDto
{
    public int Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public string Status { get; set; } = string.Empty;

    public string UserUploadUrl { get; set; } = string.Empty;

    public string? AdminProcessedUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public List<CustomStyleCommentDto> Comments { get; set; } = new();
}

public class CustomStyleCommentDto
{
    public int Id { get; set; }

    public string SenderName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsFromAdmin { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
