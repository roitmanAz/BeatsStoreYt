using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.DTOs.Catalog;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Services.Catalog;

public class BeatService : IBeatService
{
    private readonly BeatsStoreDbContext _context;

    public BeatService(BeatsStoreDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<BeatListDto>> GetBeatsAsync(
        int? styleId = null,
        int? keyboardModelId = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = _context.Beats
            .AsNoTracking()
            .Where(b => b.IsActive && b.Style.IsActive && b.KeyboardModel.IsActive);

        if (styleId.HasValue)
            query = query.Where(b => b.StyleId == styleId.Value);

        if (keyboardModelId.HasValue)
            query = query.Where(b => b.KeyboardModelId == keyboardModelId.Value);

        var totalItems = await query.CountAsync(ct);

        var beats = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BeatListDto
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Price = b.Price,
                CoverImageUrl = b.CoverImageUrl,
                DemoAudioUrl = b.DemoAudioUrl,
                StyleName = b.Style.Name,
                KeyboardModelName = b.KeyboardModel.Name,
                KeyboardBrand = b.KeyboardModel.Brand.ToString()
            })
            .ToListAsync(ct);

        return PagedResult<BeatListDto>.Create(beats, page, pageSize, totalItems);
    }

    public async Task<BeatDetailDto?> GetBeatByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Beats
            .AsNoTracking()
            .Where(b => b.Id == id && b.IsActive && b.Style.IsActive && b.KeyboardModel.IsActive)
            .Select(b => new BeatDetailDto
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Price = b.Price,
                CoverImageUrl = b.CoverImageUrl,
                DemoAudioUrl = b.DemoAudioUrl,
                IsActive = b.IsActive,
                Style = new StyleDto { Id = b.Style.Id, Name = b.Style.Name },
                KeyboardModel = new KeyboardModelDto
                {
                    Id = b.KeyboardModel.Id,
                    Brand = b.KeyboardModel.Brand.ToString(),
                    Name = b.KeyboardModel.Name
                },
                BeatSets = b.BeatSetItems
                    .Where(bsi => bsi.BeatSet.IsActive)
                    .Select(bsi => new BeatSetItemDto { Id = bsi.BeatSet.Id, Name = bsi.BeatSet.Name })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> IncrementViewCountAsync(int id, CancellationToken ct = default)
    {
        var beat = await _context.Beats
            .FirstOrDefaultAsync(b => b.Id == id && b.IsActive, ct);

        if (beat is null)
            return false;

        beat.ViewCount += 1;
        beat.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
