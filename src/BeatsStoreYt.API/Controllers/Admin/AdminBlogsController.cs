using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.DTOs.Blogs;
using BeatsStoreYt.API.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/admin/v1/blogs")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminBlogsController : BaseAdminController
{
    private readonly BeatsStoreDbContext _context;

    public AdminBlogsController(BeatsStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAll(CancellationToken ct = default)
    {
        var posts = await _context.BlogPosts
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Success(new { posts }, "פוסטים נטענו בהצלחה"));
    }

    [HttpPost("draft")]
    public async Task<ActionResult<ApiResponse<object>>> CreateDraft(
        [FromBody] CreateUpdateBlogPostDto request,
        CancellationToken ct = default)
    {
        var slugExists = await _context.BlogPosts.AnyAsync(p => p.Slug == request.Slug, ct);
        if (slugExists)
            return BadRequest(ApiResponse<object>.Failure("Slug כבר קיים במערכת"));

        var post = new Data.Features.Content.BlogPost
        {
            Title = request.Title,
            Subtitle = request.Subtitle,
            Content = request.Content,
            CoverImageUrl = request.CoverImageUrl,
            Slug = request.Slug,
            IsPublished = false,
            ViewCount = 0,
            CreatedAt = DateTimeOffset.UtcNow,
            PublishedAt = null
        };

        _context.BlogPosts.Add(post);
        await _context.SaveChangesAsync(ct);

        await WriteAdminAuditAsync("CREATE_BLOG_DRAFT", "BlogPost", post.Id.ToString(), null, new { post.Title, post.Slug }, ct);

        return Ok(ApiResponse<object>.Success(new { post }, "טיוטת פוסט נשמרה בהצלחה"));
    }

    [HttpPost("publish")]
    public async Task<ActionResult<ApiResponse<object>>> CreatePublished(
        [FromBody] CreateUpdateBlogPostDto request,
        CancellationToken ct = default)
    {
        var slugExists = await _context.BlogPosts.AnyAsync(p => p.Slug == request.Slug, ct);
        if (slugExists)
            return BadRequest(ApiResponse<object>.Failure("Slug כבר קיים במערכת"));

        var post = new Data.Features.Content.BlogPost
        {
            Title = request.Title,
            Subtitle = request.Subtitle,
            Content = request.Content,
            CoverImageUrl = request.CoverImageUrl,
            Slug = request.Slug,
            IsPublished = true,
            ViewCount = 0,
            CreatedAt = DateTimeOffset.UtcNow,
            PublishedAt = DateTimeOffset.UtcNow
        };

        _context.BlogPosts.Add(post);
        await _context.SaveChangesAsync(ct);

        await WriteAdminAuditAsync("CREATE_BLOG_PUBLISHED", "BlogPost", post.Id.ToString(), null, new { post.Title, post.Slug }, ct);

        return Ok(ApiResponse<object>.Success(new { post }, "פוסט פורסם בהצלחה"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(
        int id,
        [FromBody] CreateUpdateBlogPostDto request,
        CancellationToken ct = default)
    {
        var post = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (post is null)
            return NotFound(ApiResponse<object>.Failure("פוסט לא נמצא"));

        var slugExists = await _context.BlogPosts.AnyAsync(p => p.Slug == request.Slug && p.Id != id, ct);
        if (slugExists)
            return BadRequest(ApiResponse<object>.Failure("Slug כבר קיים במערכת"));

        var oldValues = new { post.Title, post.Subtitle, post.Slug, post.IsPublished, post.PublishedAt };

        post.Title = request.Title;
        post.Subtitle = request.Subtitle;
        post.Content = request.Content;
        post.CoverImageUrl = request.CoverImageUrl;
        post.Slug = request.Slug;

        await _context.SaveChangesAsync(ct);

        await WriteAdminAuditAsync("UPDATE_BLOG", "BlogPost", post.Id.ToString(), oldValues, new { post.Title, post.Subtitle, post.Slug, post.IsPublished, post.PublishedAt }, ct);

        return Ok(ApiResponse<object>.Success(new { post }, "פוסט עודכן בהצלחה"));
    }

    [HttpPut("{id:int}/publish-status")]
    public async Task<ActionResult<ApiResponse<object>>> UpdatePublishStatus(
        int id,
        [FromQuery] bool isPublished,
        CancellationToken ct = default)
    {
        var post = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (post is null)
            return NotFound(ApiResponse<object>.Failure("פוסט לא נמצא"));

        var oldValues = new { post.IsPublished, post.PublishedAt };

        post.IsPublished = isPublished;
        post.PublishedAt = isPublished ? DateTimeOffset.UtcNow : null;

        await _context.SaveChangesAsync(ct);

        await WriteAdminAuditAsync("UPDATE_BLOG_PUBLISH_STATUS", "BlogPost", post.Id.ToString(), oldValues, new { post.IsPublished, post.PublishedAt }, ct);

        return Ok(ApiResponse<object>.Success(new { post.Id, post.IsPublished, post.PublishedAt }, "סטטוס פרסום עודכן בהצלחה"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken ct = default)
    {
        var post = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (post is null)
            return NotFound(ApiResponse<object>.Failure("פוסט לא נמצא"));

        _context.BlogPosts.Remove(post);
        await _context.SaveChangesAsync(ct);

        await WriteAdminAuditAsync("DELETE_BLOG", "BlogPost", id.ToString(), post, null, ct);

        return Ok(ApiResponse<object>.Success(new { id }, "פוסט נמחק בהצלחה"));
    }

}
