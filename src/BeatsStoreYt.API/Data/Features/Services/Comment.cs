namespace BeatsStoreYt.API.Data.Features.Services;

public class Comment
{
    public int Id { get; set; }

    public int CustomStyleRequestId { get; set; }

    public string SenderName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsFromAdmin { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public CustomStyleRequest CustomStyleRequest { get; set; } = null!;
}
