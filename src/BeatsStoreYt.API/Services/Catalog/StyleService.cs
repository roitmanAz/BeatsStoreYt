using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.DTOs.Catalog;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Services.Catalog;

public class StyleService : IStyleService
{
    private readonly BeatsStoreDbContext _context;

    public StyleService(BeatsStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<StyleDto>> GetStylesAsync(CancellationToken ct = default)
    {
        return await _context.Styles
            .AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => new StyleDto { Id = s.Id, Name = s.Name })
            .ToListAsync(ct);
    }
}
