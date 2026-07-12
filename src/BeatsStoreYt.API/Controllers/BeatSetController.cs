using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.DTOs.Catalog;
using BeatsStoreYt.API.Services.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/beatsets")]
public class BeatSetController : ControllerBase
{
    private readonly IBeatSetService _beatSetService;
    private readonly ILogger<BeatSetController> _logger;

    public BeatSetController(IBeatSetService beatSetService, ILogger<BeatSetController> logger)
    {
        _beatSetService = beatSetService;
        _logger = logger;
    }

    /// <summary>
    /// קבל רשימת ערכות מקצבים עם דפדוף
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<BeatSetDto>>>> GetBeatSets(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        _logger.LogInformation("קבלת רשימת ערכות מקצבים: page={Page}, pageSize={PageSize}", page, pageSize);

        var result = await _beatSetService.GetBeatSetsAsync(page, pageSize, ct);

        return Ok(ApiResponse<PagedResult<BeatSetDto>>.Success(result, "ערכות מקצבים נטענו בהצלחה"));
    }

    /// <summary>
    /// קבל פרטי ערכת מקצבים יחידה
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BeatSetDetailDto>>> GetBeatSetById(
        int id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("קבלת פרטי ערכת מקצבים: id={BeatSetId}", id);

        var beatSet = await _beatSetService.GetBeatSetByIdAsync(id, ct);

        if (beatSet is null)
        {
            _logger.LogWarning("ערכת מקצבים לא נמצאה: id={BeatSetId}", id);
            return NotFound(ApiResponse<BeatSetDetailDto>.Failure($"ערכת מקצבים עם מזהה {id} לא נמצאה"));
        }

        return Ok(ApiResponse<BeatSetDetailDto>.Success(beatSet, "ערכת מקצבים נטענה בהצלחה"));
    }
}
