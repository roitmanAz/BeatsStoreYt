using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.DTOs.Catalog;

namespace BeatsStoreYt.API.Services.Catalog;

public interface IBeatService
{
    Task<PagedResult<BeatListDto>> GetBeatsAsync(
        int? styleId = null,
        int? keyboardModelId = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);

    Task<BeatDetailDto?> GetBeatByIdAsync(int id, CancellationToken ct = default);
}
