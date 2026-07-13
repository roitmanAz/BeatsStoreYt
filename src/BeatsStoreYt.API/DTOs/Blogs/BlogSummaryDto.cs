namespace BeatsStoreYt.API.DTOs.Blogs;

public class BlogSummaryDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public string CoverImageUrl { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public int ViewCount { get; set; }

    public DateTimeOffset? PublishedAt { get; set; }
}
