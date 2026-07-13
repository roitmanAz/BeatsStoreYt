using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Analytics;
using BeatsStoreYt.API.DTOs.Analytics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly BeatsStoreDbContext _context;

    public AnalyticsController(BeatsStoreDbContext context)
    {
        _context = context;
    }

    [HttpPost("log-play")]
    public async Task<ActionResult<ApiResponse<object>>> LogPlay(
        [FromBody] LogPlayEventDto request,
        CancellationToken ct = default)
    {
        if (request.BeatId <= 0)
            return BadRequest(ApiResponse<object>.Failure("נדרש מזהה מקצב תקין"));

        var beatExists = await _context.Beats
            .AsNoTracking()
            .AnyAsync(b => b.Id == request.BeatId && b.IsActive, ct);

        if (!beatExists)
            return NotFound(ApiResponse<object>.Failure("המקצב לא נמצא או לא פעיל"));

        var userId = GetUserId();
        var now = DateTimeOffset.UtcNow;
        var today = DateOnly.FromDateTime(now.UtcDateTime);
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

        _context.BeatPlayEvents.Add(new BeatPlayEvent
        {
            Id = Guid.NewGuid(),
            BeatId = request.BeatId,
            UserId = userId,
            StartedAt = now,
            PlayedSeconds = 0,
            CompletedPercent = 0,
            IpHash = ip,
            SessionId = HttpContext.Session?.Id,
            Source = "frontend-play"
        });

        var stats = await _context.BeatPlayStatsDaily
            .FirstOrDefaultAsync(s => s.BeatId == request.BeatId && s.Date == today && s.UserId == userId, ct);

        if (stats is null)
        {
            stats = new BeatPlayStatsDaily
            {
                Id = Guid.NewGuid(),
                BeatId = request.BeatId,
                Date = today,
                UserId = userId,
                PlayCount = 1,
                UniqueListeners = 1,
                TotalPlayedSeconds = 0
            };
            _context.BeatPlayStatsDaily.Add(stats);
        }
        else
        {
            stats.PlayCount += 1;
        }

        await _context.SaveChangesAsync(ct);

        return Ok(ApiResponse<object>.Success(new { beatId = request.BeatId }, "אירוע השמעה נשמר בהצלחה"));
    }

    private Guid? GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
