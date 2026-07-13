namespace BeatsStoreYt.API.DTOs.Reviews;

public class CreateEventCommentRequestDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Content { get; set; } = string.Empty;
}
