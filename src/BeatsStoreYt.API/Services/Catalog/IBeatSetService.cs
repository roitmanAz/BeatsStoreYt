using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.DTOs.Catalog;

namespace BeatsStoreYt.API.Services.Catalog;

public interface IBeatSetService
{
    Task<PagedResult<BeatSetDto>> GetBeatSetsAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);

    Task<BeatSetDetailDto?> GetBeatSetByIdAsync(int id, CancellationToken ct = default);
}
