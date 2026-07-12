using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.DTOs.Catalog;
using BeatsStoreYt.API.Services.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/catalog")]
public class CatalogController : ControllerBase
{
    private readonly IStyleService _styleService;
    private readonly IKeyboardModelService _keyboardModelService;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(
        IStyleService styleService,
        IKeyboardModelService keyboardModelService,
        ILogger<CatalogController> logger)
    {
        _styleService = styleService;
        _keyboardModelService = keyboardModelService;
        _logger = logger;
    }

    /// <summary>
    /// קבל את כל הסגנונות
    /// </summary>
    [HttpGet("styles")]
    public async Task<ActionResult<ApiResponse<List<StyleDto>>>> GetStyles(CancellationToken ct = default)
    {
        _logger.LogInformation("קבלת רשימת סגנונות");

        var styles = await _styleService.GetStylesAsync(ct);

        return Ok(ApiResponse<List<StyleDto>>.Success(styles, "סגנונות נטענו בהצלחה"));
    }

    /// <summary>
    /// קבל את כל דגמי המקלדות
    /// </summary>
    [HttpGet("keyboard-models")]
    public async Task<ActionResult<ApiResponse<List<KeyboardModelDto>>>> GetKeyboardModels(CancellationToken ct = default)
    {
        _logger.LogInformation("קבלת רשימת דגמי מקלדות");

        var models = await _keyboardModelService.GetModelsAsync(ct);

        return Ok(ApiResponse<List<KeyboardModelDto>>.Success(models, "דגמי מקלדות נטענו בהצלחה"));
    }
}
