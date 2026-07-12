using BeatsStoreYt.API.DTOs.Catalog;

namespace BeatsStoreYt.API.Services.Catalog;

public interface IKeyboardModelService
{
    Task<List<KeyboardModelDto>> GetModelsAsync(CancellationToken ct = default);
}
