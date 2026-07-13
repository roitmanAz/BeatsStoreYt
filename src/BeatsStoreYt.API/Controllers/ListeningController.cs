using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.DTOs.Storage;
using BeatsStoreYt.API.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/listening")]
// Legacy SAS-based access path kept for future protected/private files.
public class ListeningController : ControllerBase
{
    private readonly IAzureBlobStorageService _blobStorage;

    public ListeningController(IAzureBlobStorageService blobStorage)
    {
        _blobStorage = blobStorage;
    }

    [HttpPost("request")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<object>>> RequestListeningUrl(
        [FromBody] ListeningRequestDto request,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.BlobPath))
            return BadRequest(ApiResponse<object>.Failure("נתיב קובץ חובה"));

        var validMinutes = request.ValidMinutes <= 0 ? 10 : Math.Min(request.ValidMinutes, 60);
        var url = await _blobStorage.GetReadUrlAsync(request.BlobPath, TimeSpan.FromMinutes(validMinutes), ct);

        return Ok(ApiResponse<object>.Success(new
        {
            url,
            expiresInMinutes = validMinutes
        }, "קישור שמיעה נוצר בהצלחה"));
    }
}
