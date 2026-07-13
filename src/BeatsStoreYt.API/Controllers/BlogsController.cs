using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.DTOs.Blogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/blogs")]
public class BlogsController : ControllerBase
{
    private readonly BeatsStoreDbContext _context;

    public BlogsController(BeatsStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<BlogSummaryDto>>>> GetPublishedBlogs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _context.BlogPosts
            .AsNoTracking()
            .Where(p => p.IsPublished);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.PublishedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new BlogSummaryDto
            {
                Id = p.Id,
                Title = p.Title,
                Subtitle = p.Subtitle,
                CoverImageUrl = p.CoverImageUrl,
                Slug = p.Slug,
                ViewCount = p.ViewCount,
                PublishedAt = p.PublishedAt
            })
            .ToListAsync(ct);

        var result = PagedResult<BlogSummaryDto>.Create(items, page, pageSize, totalItems);
        return Ok(ApiResponse<PagedResult<BlogSummaryDto>>.Success(result, "פוסטים פומביים נטענו בהצלחה"));
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ApiResponse<BlogDetailDto>>> GetPublishedBlogBySlug(
        string slug,
        CancellationToken ct = default)
    {
        var post = await _context.BlogPosts
            .FirstOrDefaultAsync(p => p.Slug == slug, ct);

        if (post is null || !post.IsPublished)
            return NotFound(ApiResponse<BlogDetailDto>.Failure("פוסט לא נמצא"));

        post.ViewCount += 1;
        await _context.SaveChangesAsync(ct);

        var dto = new BlogDetailDto
        {
            Id = post.Id,
            Title = post.Title,
            Subtitle = post.Subtitle,
            CoverImageUrl = post.CoverImageUrl,
            Slug = post.Slug,
            ViewCount = post.ViewCount,
            PublishedAt = post.PublishedAt,
            Content = post.Content
        };

        return Ok(ApiResponse<BlogDetailDto>.Success(dto, "פוסט נטען בהצלחה"));
    }
}
