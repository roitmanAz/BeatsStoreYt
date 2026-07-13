namespace BeatsStoreYt.API.DTOs.Reviews;

public class SiteReviewDto
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
}
