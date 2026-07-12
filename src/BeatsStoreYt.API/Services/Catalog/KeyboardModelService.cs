using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.DTOs.Catalog;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Services.Catalog;

public class KeyboardModelService : IKeyboardModelService
{
    private readonly BeatsStoreDbContext _context;

    public KeyboardModelService(BeatsStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<KeyboardModelDto>> GetModelsAsync(CancellationToken ct = default)
    {
        return await _context.KeyboardModels
            .AsNoTracking()
            .Where(k => k.IsActive)
            .OrderBy(k => k.Brand)
            .ThenBy(k => k.Name)
            .Select(k => new KeyboardModelDto
            {
                Id = k.Id,
                Brand = k.Brand.ToString(),
                Name = k.Name
            })
            .ToListAsync(ct);
    }
}
