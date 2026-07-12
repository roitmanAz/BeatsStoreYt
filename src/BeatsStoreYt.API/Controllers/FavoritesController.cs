using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Commerce.Common;
using BeatsStoreYt.API.Data.Features.Commerce.Favorites;
using BeatsStoreYt.API.DTOs.Favorites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/favorites")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly BeatsStoreDbContext _context;

    public FavoritesController(BeatsStoreDbContext context)
    {
        _context = context;
    }

    [HttpPost("beats")]
    public async Task<ActionResult<ApiResponse<object>>> AddBeatToFavorites(
        [FromBody] AddBeatToFavoritesRequestDto request,
        CancellationToken ct = default)
    {
        if (request.BeatId <= 0)
            return BadRequest(ApiResponse<object>.Failure("נדרש מזהה מקצב תקין"));

        var userId = GetRequiredUserId();
        if (userId is null)
            return Unauthorized(ApiResponse<object>.Failure("משתמש לא מזוהה"));

        var beat = await _context.Beats
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == request.BeatId && b.IsActive, ct);

        if (beat is null)
            return NotFound(ApiResponse<object>.Failure("המקצב לא נמצא או לא פעיל"));

        var favorite = await _context.Favorites
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.UserId == userId, ct);

        if (favorite is null)
        {
            favorite = new Favorite
            {
                Id = Guid.NewGuid(),
                UserId = userId.Value,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            _context.Favorites.Add(favorite);
        }

        var existingItem = favorite.Items
            .FirstOrDefault(i => i.ProductType == CatalogProductType.Beat && i.ProductId == request.BeatId);

        if (existingItem is null)
        {
            favorite.Items.Add(new FavoriteItem
            {
                Id = Guid.NewGuid(),
                FavoriteId = favorite.Id,
                ProductType = CatalogProductType.Beat,
                ProductId = request.BeatId,
                AddedAt = DateTimeOffset.UtcNow
            });
        }

        favorite.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(ct);

        return Ok(ApiResponse<object>.Success(new
        {
            favoriteId = favorite.Id,
            productType = CatalogProductType.Beat.ToString(),
            productId = request.BeatId,
            alreadyExists = existingItem is not null
        }, existingItem is null ? "המקצב נוסף למועדפים בהצלחה" : "המקצב כבר קיים במועדפים"));
    }

    [HttpGet("mine")]
    public async Task<ActionResult<ApiResponse<object>>> GetMyFavorites(CancellationToken ct = default)
    {
        var userId = GetRequiredUserId();
        if (userId is null)
            return Unauthorized(ApiResponse<object>.Failure("משתמש לא מזוהה"));

        var favorites = await _context.Favorites
            .AsNoTracking()
            .Where(f => f.UserId == userId)
            .Select(f => new
            {
                f.Id,
                f.CreatedAt,
                f.UpdatedAt,
                items = f.Items
                    .OrderByDescending(i => i.AddedAt)
                    .Select(i => new
                    {
                        i.Id,
                        productType = i.ProductType.ToString(),
                        i.ProductId,
                        i.AddedAt,
                        beatTitle = i.ProductType == CatalogProductType.Beat
                            ? _context.Beats.Where(b => b.Id == i.ProductId).Select(b => b.Title).FirstOrDefault()
                            : null,
                        beatDemoAudioUrl = i.ProductType == CatalogProductType.Beat
                            ? _context.Beats.Where(b => b.Id == i.ProductId).Select(b => b.DemoAudioUrl).FirstOrDefault()
                            : null,
                        beatCoverImageUrl = i.ProductType == CatalogProductType.Beat
                            ? _context.Beats.Where(b => b.Id == i.ProductId).Select(b => b.CoverImageUrl).FirstOrDefault()
                            : null,
                        beatPrice = i.ProductType == CatalogProductType.Beat
                            ? _context.Beats.Where(b => b.Id == i.ProductId).Select(b => (decimal?)b.Price).FirstOrDefault()
                            : null
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (favorites is null)
            return Ok(ApiResponse<object>.Success(new { }, "לא נמצאו מועדפים למשתמש"));

        return Ok(ApiResponse<object>.Success(new { favorites }, "רשימת המועדפים נטענה בהצלחה"));
    }

    private Guid? GetRequiredUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
