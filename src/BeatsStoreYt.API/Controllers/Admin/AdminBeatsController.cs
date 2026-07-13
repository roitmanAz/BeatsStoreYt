using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Catalog;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.DTOs.Admin;
using BeatsStoreYt.API.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/admin/v1/beats")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminBeatsController : BaseAdminController
{
    private readonly BeatsStoreDbContext _context;

    public AdminBeatsController(BeatsStoreDbContext context, IAuditLogService audit)
        : base(audit)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAll(CancellationToken ct = default)
    {
        var beats = await _context.Beats
            .AsNoTracking()
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Success(new { beats }, "ביטים נטענו בהצלחה"));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] AdminCreateBeatDto request, CancellationToken ct = default)
    {
        var hasStyle = await _context.Styles.AnyAsync(s => s.Id == request.StyleId, ct);
        var hasKeyboardModel = await _context.KeyboardModels.AnyAsync(k => k.Id == request.KeyboardModelId, ct);

        if (!hasStyle || !hasKeyboardModel)
            return BadRequest(ApiResponse<object>.Failure("סגנון או מודל אורגן לא קיים במערכת"));

        var beat = new Beat
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            KeyboardModelId = request.KeyboardModelId,
            StyleId = request.StyleId,
            CoverImageUrl = request.CoverImageUrl,
            DemoAudioUrl = request.DemoAudioUrl,
            ProductFileStorageKey = request.ProductFileStorageKey,
            WaveformPeaks = request.WaveformPeaks,
            IsActive = request.IsActive,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Beats.Add(beat);
        await _context.SaveChangesAsync(ct);

        await WriteAdminAuditAsync("CREATE_BEAT", "Beat", beat.Id.ToString(), null, new { beat.Title }, ct);

        return Ok(ApiResponse<object>.Success(new { beat }, "ביט נוצר בהצלחה"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] AdminUpdateBeatDto request, CancellationToken ct = default)
    {
        var beat = await _context.Beats.FirstOrDefaultAsync(b => b.Id == id, ct);
        if (beat is null)
            return NotFound(ApiResponse<object>.Failure("ביט לא נמצא"));

        var oldValues = new
        {
            beat.Title,
            beat.Description,
            beat.Price,
            beat.KeyboardModelId,
            beat.StyleId,
            beat.CoverImageUrl,
            beat.DemoAudioUrl,
            beat.ProductFileStorageKey,
            beat.WaveformPeaks,
            beat.IsActive
        };

        beat.Title = request.Title;
        beat.Description = request.Description;
        beat.Price = request.Price;
        beat.KeyboardModelId = request.KeyboardModelId;
        beat.StyleId = request.StyleId;
        beat.CoverImageUrl = request.CoverImageUrl;
        beat.DemoAudioUrl = request.DemoAudioUrl;
        beat.ProductFileStorageKey = request.ProductFileStorageKey;
        beat.WaveformPeaks = request.WaveformPeaks;
        beat.IsActive = request.IsActive;
        beat.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(ct);
        await WriteAdminAuditAsync("UPDATE_BEAT", "Beat", beat.Id.ToString(), oldValues, beat, ct);

        return Ok(ApiResponse<object>.Success(new { beat }, "ביט עודכן בהצלחה"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken ct = default)
    {
        var beat = await _context.Beats.FirstOrDefaultAsync(b => b.Id == id, ct);
        if (beat is null)
            return NotFound(ApiResponse<object>.Failure("ביט לא נמצא"));

        _context.Beats.Remove(beat);
        await _context.SaveChangesAsync(ct);
        await WriteAdminAuditAsync("DELETE_BEAT", "Beat", id.ToString(), beat, null, ct);

        return Ok(ApiResponse<object>.Success(new { id }, "ביט נמחק בהצלחה"));
    }
}
