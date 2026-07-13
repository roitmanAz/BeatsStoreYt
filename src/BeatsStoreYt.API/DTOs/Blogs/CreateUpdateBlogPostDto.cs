namespace BeatsStoreYt.API.DTOs.Blogs;

public class CreateUpdateBlogPostDto
{
    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string CoverImageUrl { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;
}
