using BeatsStoreYt.API.DTOs.Catalog;

namespace BeatsStoreYt.API.Services.Catalog;

public interface IStyleService
{
    Task<List<StyleDto>> GetStylesAsync(CancellationToken ct = default);
}
