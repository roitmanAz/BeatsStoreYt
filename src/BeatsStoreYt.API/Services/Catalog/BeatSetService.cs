using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.DTOs.Catalog;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Services.Catalog;

public class BeatSetService : IBeatSetService
{
    private readonly BeatsStoreDbContext _context;

    public BeatSetService(BeatsStoreDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<BeatSetDto>> GetBeatSetsAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = _context.BeatSets
            .AsNoTracking()
            .Where(bs => bs.IsActive);

        var totalItems = await query.CountAsync(ct);

        var beatSets = await query
            .OrderBy(b => b.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BeatSetDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Price = b.Price,
                CoverImageUrl = b.CoverImageUrl,
                DemoAudioUrl = b.DemoAudioUrl,
                ItemsCount = b.BeatSetItems.Count(i => i.Beat.IsActive),
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .ToListAsync(ct);

        return PagedResult<BeatSetDto>.Create(beatSets, page, pageSize, totalItems);
    }

    public async Task<BeatSetDetailDto?> GetBeatSetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.BeatSets
            .AsNoTracking()
            .Where(bs => bs.Id == id && bs.IsActive)
            .Select(bs => new BeatSetDetailDto
            {
                Id = bs.Id,
                Name = bs.Name,
                Description = bs.Description,
                Price = bs.Price,
                CoverImageUrl = bs.CoverImageUrl,
                DemoAudioUrl = bs.DemoAudioUrl,
                ItemsCount = bs.BeatSetItems.Count(bsi => bsi.Beat.IsActive),
                IsActive = bs.IsActive,
                CreatedAt = bs.CreatedAt,
                UpdatedAt = bs.UpdatedAt,
                Items = bs.BeatSetItems
                    .Where(bsi => bsi.Beat.IsActive)
                    .OrderBy(bsi => bsi.SortOrder)
                    .Select(bsi => new BeatInSetDto
                    {
                        Id = bsi.Beat.Id,
                        Title = bsi.Beat.Title,
                        Description = bsi.Beat.Description,
                        Price = bsi.Beat.Price,
                        CoverImageUrl = bsi.Beat.CoverImageUrl,
                        DemoAudioUrl = bsi.Beat.DemoAudioUrl,
                        ProductFileStorageKey = bsi.Beat.ProductFileStorageKey,
                        WaveformPeaks = bsi.Beat.WaveformPeaks,
                        StyleName = bsi.Beat.Style.Name,
                        SortOrder = bsi.SortOrder
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
    }
}
