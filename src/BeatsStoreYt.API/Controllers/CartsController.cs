using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Commerce.Common;
using BeatsStoreYt.API.Data.Features.Commerce.Cart;
using BeatsStoreYt.API.DTOs.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/carts")]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly BeatsStoreDbContext _context;

    public CartsController(BeatsStoreDbContext context)
    {
        _context = context;
    }

    [HttpPost("items/beatset")]
    public async Task<ActionResult<ApiResponse<object>>> AddBeatSetToCart(
        [FromBody] AddBeatSetToCartRequestDto request,
        CancellationToken ct = default)
    {
        if (request.BeatSetId <= 0)
            return BadRequest(ApiResponse<object>.Failure("נדרש מזהה סט תקין"));

        if (request.Quantity <= 0)
            return BadRequest(ApiResponse<object>.Failure("כמות חייבת להיות גדולה מאפס"));

        var userId = GetRequiredUserId();
        if (userId is null)
            return Unauthorized(ApiResponse<object>.Failure("משתמש לא מזוהה"));

        var beatSet = await _context.BeatSets
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == request.BeatSetId && b.IsActive, ct);

        if (beatSet is null)
            return NotFound(ApiResponse<object>.Failure("סט לא נמצא או לא פעיל"));

        var cart = await _context.ShoppingCarts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

        if (cart is null)
        {
            cart = new ShoppingCart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Currency = "ILS",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            _context.ShoppingCarts.Add(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductType == CatalogProductType.BeatSet && i.ProductId == request.BeatSetId);

        if (existingItem is null)
        {
            cart.Items.Add(new CartItem
            {
                Id = Guid.NewGuid(),
                ShoppingCartId = cart.Id,
                ProductType = CatalogProductType.BeatSet,
                ProductId = request.BeatSetId,
                UnitPriceSnapshot = beatSet.Price,
                Quantity = request.Quantity,
                AddedAt = DateTimeOffset.UtcNow
            });
        }
        else
        {
            existingItem.Quantity += request.Quantity;
            existingItem.UnitPriceSnapshot = beatSet.Price;
        }

        cart.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(ct);

        return Ok(ApiResponse<object>.Success(new
        {
            cartId = cart.Id,
            productType = CatalogProductType.BeatSet.ToString(),
            productId = request.BeatSetId,
            quantity = existingItem?.Quantity ?? request.Quantity
        }, "הסט נוסף לעגלת הקניות בהצלחה"));
    }

    [HttpGet("mine")]
    public async Task<ActionResult<ApiResponse<object>>> GetMySavedCart(CancellationToken ct = default)
    {
        var userId = GetRequiredUserId();
        if (userId is null)
            return Unauthorized(ApiResponse<object>.Failure("משתמש לא מזוהה"));

        var cart = await _context.ShoppingCarts
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .Select(c => new
            {
                c.Id,
                c.Currency,
                c.CreatedAt,
                c.UpdatedAt,
                items = c.Items
                    .OrderByDescending(i => i.AddedAt)
                    .Select(i => new
                    {
                        i.Id,
                        productType = i.ProductType.ToString(),
                        i.ProductId,
                        i.UnitPriceSnapshot,
                        i.Quantity,
                        i.AddedAt,
                        beatSetName = i.ProductType == CatalogProductType.BeatSet
                            ? _context.BeatSets.Where(bs => bs.Id == i.ProductId).Select(bs => bs.Name).FirstOrDefault()
                            : null,
                        beatSetCoverImageUrl = i.ProductType == CatalogProductType.BeatSet
                            ? _context.BeatSets.Where(bs => bs.Id == i.ProductId).Select(bs => bs.CoverImageUrl).FirstOrDefault()
                            : null
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (cart is null)
            return Ok(ApiResponse<object>.Success(new { }, "לא נמצאה עגלת קניות שמורה למשתמש"));

        return Ok(ApiResponse<object>.Success(new { cart }, "עגלת הקניות נטענה בהצלחה"));
    }

    private Guid? GetRequiredUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
