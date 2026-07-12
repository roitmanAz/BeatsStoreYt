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
                DemoAudioUrl = b.DemoAudioUrl
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
                Items = bs.BeatSetItems
                    .Where(bsi => bsi.Beat.IsActive)
                    .OrderBy(bsi => bsi.SortOrder)
                    .Select(bsi => new BeatInSetDto
                    {
                        Id = bsi.Beat.Id,
                        Title = bsi.Beat.Title,
                        Price = bsi.Beat.Price,
                        DemoAudioUrl = bsi.Beat.DemoAudioUrl,
                        StyleName = bsi.Beat.Style.Name
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
    }
}
