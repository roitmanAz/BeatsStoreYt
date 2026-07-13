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
[Route("api/admin/v1/beatsets")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminBeatSetsController : BaseAdminController
{
    private readonly BeatsStoreDbContext _context;

    public AdminBeatSetsController(BeatsStoreDbContext context, IAuditLogService audit)
        : base(audit)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetAll(CancellationToken ct = default)
    {
        var beatSets = await _context.BeatSets
            .AsNoTracking()
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Success(new { beatSets }, "סטים נטענו בהצלחה"));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] AdminCreateBeatSetDto request, CancellationToken ct = default)
    {
        var beatSet = new BeatSet
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CoverImageUrl = request.CoverImageUrl,
            DemoAudioUrl = request.DemoAudioUrl,
            IsActive = request.IsActive,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.BeatSets.Add(beatSet);
        await _context.SaveChangesAsync(ct);
        await WriteAdminAuditAsync("CREATE_BEAT_SET", "BeatSet", beatSet.Id.ToString(), null, new { beatSet.Name }, ct);

        return Ok(ApiResponse<object>.Success(new { beatSet }, "סט נוצר בהצלחה"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] AdminUpdateBeatSetDto request, CancellationToken ct = default)
    {
        var beatSet = await _context.BeatSets.FirstOrDefaultAsync(b => b.Id == id, ct);
        if (beatSet is null)
            return NotFound(ApiResponse<object>.Failure("סט לא נמצא"));

        var oldValues = new
        {
            beatSet.Name,
            beatSet.Description,
            beatSet.Price,
            beatSet.CoverImageUrl,
            beatSet.DemoAudioUrl,
            beatSet.IsActive
        };

        beatSet.Name = request.Name;
        beatSet.Description = request.Description;
        beatSet.Price = request.Price;
        beatSet.CoverImageUrl = request.CoverImageUrl;
        beatSet.DemoAudioUrl = request.DemoAudioUrl;
        beatSet.IsActive = request.IsActive;
        beatSet.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(ct);
        await WriteAdminAuditAsync("UPDATE_BEAT_SET", "BeatSet", beatSet.Id.ToString(), oldValues, beatSet, ct);

        return Ok(ApiResponse<object>.Success(new { beatSet }, "סט עודכן בהצלחה"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken ct = default)
    {
        var beatSet = await _context.BeatSets.FirstOrDefaultAsync(b => b.Id == id, ct);
        if (beatSet is null)
            return NotFound(ApiResponse<object>.Failure("סט לא נמצא"));

        _context.BeatSets.Remove(beatSet);
        await _context.SaveChangesAsync(ct);
        await WriteAdminAuditAsync("DELETE_BEAT_SET", "BeatSet", id.ToString(), beatSet, null, ct);

        return Ok(ApiResponse<object>.Success(new { id }, "סט נמחק בהצלחה"));
    }
}
