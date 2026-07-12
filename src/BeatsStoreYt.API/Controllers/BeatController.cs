using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.DTOs.Catalog;
using BeatsStoreYt.API.Services.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/beats")]
public class BeatController : ControllerBase
{
    private readonly IBeatService _beatService;
    private readonly ILogger<BeatController> _logger;

    public BeatController(IBeatService beatService, ILogger<BeatController> logger)
    {
        _beatService = beatService;
        _logger = logger;
    }

    /// <summary>
    /// קבל רשימת מקצבים עם סינון ודפדוף
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<BeatListDto>>>> GetBeats(
        [FromQuery] int? styleId = null,
        [FromQuery] int? keyboardModelId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        _logger.LogInformation("קבלת רשימת מקצבים: styleId={StyleId}, keyboardModelId={KeyboardModelId}, page={Page}, pageSize={PageSize}",
            styleId, keyboardModelId, page, pageSize);

        var result = await _beatService.GetBeatsAsync(styleId, keyboardModelId, page, pageSize, ct);

        return Ok(ApiResponse<PagedResult<BeatListDto>>.Success(result, "מקצבים נטענו בהצלחה"));
    }

    /// <summary>
    /// קבל פרטי מקצב יחיד
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BeatDetailDto>>> GetBeatById(
        int id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("קבלת פרטי מקצב: id={BeatId}", id);

        var beat = await _beatService.GetBeatByIdAsync(id, ct);

        if (beat is null)
        {
            _logger.LogWarning("מקצב לא נמצא: id={BeatId}", id);
            return NotFound(ApiResponse<BeatDetailDto>.Failure($"מקצב עם מזהה {id} לא נמצא"));
        }

        return Ok(ApiResponse<BeatDetailDto>.Success(beat, "מקצב נטען בהצלחה"));
    }
}
