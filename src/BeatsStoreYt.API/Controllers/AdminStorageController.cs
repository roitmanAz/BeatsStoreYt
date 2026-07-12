using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Catalog;
using BeatsStoreYt.API.Data.Features.Content;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.DTOs.Admin;
using BeatsStoreYt.API.DTOs.Storage;
using BeatsStoreYt.API.Services.Admin;
using BeatsStoreYt.API.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/admin/storage")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminStorageController : ControllerBase
{
    private static readonly string[] BeatFileExtensions = [".zip", ".rar", ".7z", ".bpk", ".sty"];
    private static readonly string[] PreviewAudioExtensions = [".mp3", ".wav", ".m4a", ".aac", ".ogg", ".flac"];
    private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif", ".bmp"];

    private readonly BeatsStoreDbContext _context;
    private readonly IAzureBlobStorageService _blobStorage;
    private readonly IAuditLogService _audit;

    public AdminStorageController(BeatsStoreDbContext context, IAzureBlobStorageService blobStorage, IAuditLogService audit)
    {
        _context = context;
        _blobStorage = blobStorage;
        _audit = audit;
    }

    [HttpPost("upload-file")]
    [RequestSizeLimit(200_000_000)]
    public async Task<ActionResult<ApiResponse<object>>> UploadFile(
        [FromForm] IFormFile file,
        [FromForm] string? folderPath,
        [FromForm] int? beatId = null,
        CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            return BadRequest(ApiResponse<object>.Failure("לא נבחר קובץ להעלאה"));

        var safeFileName = Path.GetFileName(file.FileName);
        var blobPath = string.IsNullOrWhiteSpace(folderPath)
            ? safeFileName
            : $"{folderPath!.TrimEnd('/', '\\')}/{safeFileName}";

        var linkedBeatResult = await ResolveBeatOrErrorAsync(beatId, blobPath, ct);
        if (linkedBeatResult.error is not null)
            return linkedBeatResult.error;

        var linkedBeat = linkedBeatResult.beat;
        var existedBeforeUpload = await _blobStorage.ExistsAsync(blobPath, ct);

        await using var stream = file.OpenReadStream();
        var storedPath = await _blobStorage.UploadFileAsync(new BlobFileUploadRequest
        {
            BlobPath = blobPath,
            ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            Content = stream
        }, ct);

        var existingAsset = await _context.MediaAssets
            .FirstOrDefaultAsync(m => m.BlobStorageKey == storedPath, ct);

        var now = DateTimeOffset.UtcNow;
        var mediaType = ResolveMediaType(file, storedPath);
        var title = Path.GetFileNameWithoutExtension(safeFileName);

        var asset = existingAsset;
        if (asset is null)
        {
            asset = new MediaAsset
            {
                Id = Guid.NewGuid(),
                Title = title,
                MediaType = mediaType,
                BlobStorageKey = storedPath,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            };
            _context.MediaAssets.Add(asset);
        }
        else
        {
            asset.Title = title;
            asset.MediaType = mediaType;
            asset.BlobStorageKey = storedPath;
            asset.IsActive = true;
            asset.UpdatedAt = now;
        }

        if (linkedBeat is not null)
            UpdateBeatStorageFields(linkedBeat, mediaType, storedPath, now);

        await _context.SaveChangesAsync(ct);

        await _audit.WriteAsync(
            GetUserId(),
            "UPLOAD_FILE",
            "Blob",
            storedPath,
            null,
            new { storedPath, mediaAssetId = asset.Id, beatId = linkedBeat?.Id, overwritten = existedBeforeUpload || existingAsset is not null },
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);

        return Ok(ApiResponse<object>.Success(
            new
            {
                blobPath = storedPath,
                mediaAssetId = asset.Id,
                beatId = linkedBeat?.Id,
                overwritten = existedBeforeUpload || existingAsset is not null
            },
            "הקובץ הועלה ונשמר בדאטה בייס בהצלחה"));
    }

    [HttpPost("upload-files")]
    [RequestSizeLimit(500_000_000)]
    public async Task<ActionResult<ApiResponse<object>>> UploadFiles(
        [FromForm] List<IFormFile> files,
        [FromForm] string? folderPath,
        [FromForm] int? beatId = null,
        CancellationToken ct = default)
    {
        if (files.Count == 0)
            return BadRequest(ApiResponse<object>.Failure("לא נבחרו קבצים להעלאה"));

        var linkedBeatResult = await ResolveBeatOrErrorAsync(beatId, null, ct);
        if (linkedBeatResult.error is not null)
            return linkedBeatResult.error;

        var linkedBeat = linkedBeatResult.beat;
        var uploaded = new List<object>();

        foreach (var file in files)
        {
            if (file.Length == 0)
                continue;

            var safeFileName = Path.GetFileName(file.FileName);
            var blobPath = string.IsNullOrWhiteSpace(folderPath)
                ? safeFileName
                : $"{folderPath!.TrimEnd('/', '\\')}/{safeFileName}";

            var existedBeforeUpload = await _blobStorage.ExistsAsync(blobPath, ct);

            await using var stream = file.OpenReadStream();
            var storedPath = await _blobStorage.UploadFileAsync(new BlobFileUploadRequest
            {
                BlobPath = blobPath,
                ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
                Content = stream
            }, ct);

            var existingAsset = await _context.MediaAssets
                .FirstOrDefaultAsync(m => m.BlobStorageKey == storedPath, ct);

            var now = DateTimeOffset.UtcNow;
            var mediaType = ResolveMediaType(file, storedPath);
            var title = Path.GetFileNameWithoutExtension(safeFileName);

            var asset = existingAsset;
            if (asset is null)
            {
                asset = new MediaAsset
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    MediaType = mediaType,
                    BlobStorageKey = storedPath,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                _context.MediaAssets.Add(asset);
            }
            else
            {
                asset.Title = title;
                asset.MediaType = mediaType;
                asset.BlobStorageKey = storedPath;
                asset.IsActive = true;
                asset.UpdatedAt = now;
            }

            if (linkedBeat is not null)
                UpdateBeatStorageFields(linkedBeat, mediaType, storedPath, now);

            uploaded.Add(new
            {
                blobPath = storedPath,
                mediaAssetId = asset.Id,
                mediaType,
                overwritten = existedBeforeUpload || existingAsset is not null
            });
        }

        await _context.SaveChangesAsync(ct);

        await _audit.WriteAsync(
            GetUserId(),
            "UPLOAD_FILES",
            "BlobBatch",
            folderPath ?? string.Empty,
            null,
            new { uploadedCount = uploaded.Count, beatId = linkedBeat?.Id, uploaded },
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            ct);

        return Ok(ApiResponse<object>.Success(new { uploaded, beatId = linkedBeat?.Id }, "הקבצים הועלו ונשמרו בדאטה בייס בהצלחה"));
    }

    [HttpPost("create-folder")]
    public async Task<ActionResult<ApiResponse<object>>> CreateFolder(
        [FromBody] AdminCreateFolderDto request,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.FolderPath))
            return BadRequest(ApiResponse<object>.Failure("נתיב תיקייה חובה"));

        var folder = await _blobStorage.CreateFolderAsync(request.FolderPath, ct);
        await _audit.WriteAsync(GetUserId(), "CREATE_FOLDER", "BlobFolder", folder, null, new { folder }, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        return Ok(ApiResponse<object>.Success(new { folder }, "התיקייה נוצרה בהצלחה"));
    }

    [HttpDelete("delete-file")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteFile(
        [FromBody] AdminDeleteFileDto request,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.BlobPath))
            return BadRequest(ApiResponse<object>.Failure("נתיב קובץ חובה"));

        await _blobStorage.DeleteFileAsync(request.BlobPath, ct);
        await _audit.WriteAsync(GetUserId(), "DELETE_FILE", "Blob", request.BlobPath, new { request.BlobPath }, null, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        return Ok(ApiResponse<object>.Success(new { }, "הקובץ נמחק בהצלחה"));
    }

    [HttpDelete("delete-folder")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteFolder(
        [FromBody] AdminDeleteFolderDto request,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.FolderPath))
            return BadRequest(ApiResponse<object>.Failure("נתיב תיקייה חובה"));

        var result = await _blobStorage.DeleteFolderAsync(request.FolderPath, ct);
        await _audit.WriteAsync(GetUserId(), "DELETE_FOLDER", "BlobFolder", request.FolderPath, new { request.FolderPath }, result, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        return Ok(ApiResponse<object>.Success(new { result }, "התיקייה נמחקה בהצלחה"));
    }

    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<object>>> ListFiles(
        [FromQuery] string? folderPath,
        CancellationToken ct = default)
    {
        var files = await _blobStorage.ListFilesAsync(folderPath, ct);
        return Ok(ApiResponse<object>.Success(new { files }, "רשימת קבצים נטענה בהצלחה"));
    }

    [HttpGet("tree")]
    public async Task<ActionResult<ApiResponse<object>>> ListTree(
        [FromQuery] string? folderPath,
        CancellationToken ct = default)
    {
        var result = await _blobStorage.ListTreeAsync(folderPath, ct);
        return Ok(ApiResponse<object>.Success(new { result }, "עץ תיקיות וקבצים נטען בהצלחה"));
    }

    private async Task<(Beat? beat, ActionResult<ApiResponse<object>>? error)> ResolveBeatOrErrorAsync(
        int? beatId,
        string? blobPath,
        CancellationToken ct)
    {
        if (beatId.HasValue)
        {
            var beat = await _context.Beats.FirstOrDefaultAsync(b => b.Id == beatId.Value, ct);
            if (beat is null)
                return (null, NotFound(ApiResponse<object>.Failure($"ביט עם מזהה {beatId.Value} לא נמצא")));

            return (beat, null);
        }

        if (string.IsNullOrWhiteSpace(blobPath))
            return (null, null);

        var linked = await _context.Beats
            .FirstOrDefaultAsync(b => b.ProductFileStorageKey == blobPath || b.DemoAudioUrl == blobPath || b.CoverImageUrl == blobPath, ct);

        return (linked, null);
    }

    private static void UpdateBeatStorageFields(Beat beat, string mediaType, string blobPath, DateTimeOffset now)
    {
        if (mediaType == "audio")
            beat.DemoAudioUrl = blobPath;
        else if (mediaType == "image")
            beat.CoverImageUrl = blobPath;
        else
            beat.ProductFileStorageKey = blobPath;

        beat.UpdatedAt = now;
    }

    private static string ResolveMediaType(IFormFile file, string blobPath)
    {
        var extension = Path.GetExtension(blobPath).ToLowerInvariant();
        if (ImageExtensions.Contains(extension))
            return "image";

        if (PreviewAudioExtensions.Contains(extension))
            return "audio";

        if (BeatFileExtensions.Contains(extension))
            return "beat";

        if (!string.IsNullOrWhiteSpace(file.ContentType) && file.ContentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
            return "audio";

        if (!string.IsNullOrWhiteSpace(file.ContentType) && file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            return "image";

        return "file";
    }

    private Guid? GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var guid) ? guid : null;
    }
}
