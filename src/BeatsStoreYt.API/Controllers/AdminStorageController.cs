using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.DTOs.Admin;
using BeatsStoreYt.API.DTOs.Storage;
using BeatsStoreYt.API.Services.Admin;
using BeatsStoreYt.API.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/admin/storage")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminStorageController : ControllerBase
{
    private readonly IAzureBlobStorageService _blobStorage;
    private readonly IAuditLogService _audit;

    public AdminStorageController(IAzureBlobStorageService blobStorage, IAuditLogService audit)
    {
        _blobStorage = blobStorage;
        _audit = audit;
    }

    [HttpPost("upload-file")]
    [RequestSizeLimit(200_000_000)]
    public async Task<ActionResult<ApiResponse<object>>> UploadFile(
        [FromForm] IFormFile file,
        [FromForm] string? folderPath,
        CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            return BadRequest(ApiResponse<object>.Failure("לא נבחר קובץ להעלאה"));

        var safeFileName = Path.GetFileName(file.FileName);
        var blobPath = string.IsNullOrWhiteSpace(folderPath)
            ? safeFileName
            : $"{folderPath!.TrimEnd('/', '\\')}/{safeFileName}";

        await using var stream = file.OpenReadStream();
        var storedPath = await _blobStorage.UploadFileAsync(new BlobFileUploadRequest
        {
            BlobPath = blobPath,
            ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            Content = stream
        }, ct);

        await _audit.WriteAsync(GetUserId(), "UPLOAD_FILE", "Blob", storedPath, null, new { storedPath }, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);

        return Ok(ApiResponse<object>.Success(new { blobPath = storedPath }, "הקובץ הועלה בהצלחה"));
    }

    [HttpPost("upload-files")]
    [RequestSizeLimit(500_000_000)]
    public async Task<ActionResult<ApiResponse<object>>> UploadFiles(
        [FromForm] List<IFormFile> files,
        [FromForm] string? folderPath,
        CancellationToken ct = default)
    {
        if (files.Count == 0)
            return BadRequest(ApiResponse<object>.Failure("לא נבחרו קבצים להעלאה"));

        var uploaded = new List<string>();

        foreach (var file in files)
        {
            if (file.Length == 0)
                continue;

            var safeFileName = Path.GetFileName(file.FileName);
            var blobPath = string.IsNullOrWhiteSpace(folderPath)
                ? safeFileName
                : $"{folderPath!.TrimEnd('/', '\\')}/{safeFileName}";

            await using var stream = file.OpenReadStream();
            var storedPath = await _blobStorage.UploadFileAsync(new BlobFileUploadRequest
            {
                BlobPath = blobPath,
                ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
                Content = stream
            }, ct);

            uploaded.Add(storedPath);
        }

        await _audit.WriteAsync(GetUserId(), "UPLOAD_FILES", "BlobBatch", folderPath ?? string.Empty, null, new { uploaded }, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);

        return Ok(ApiResponse<object>.Success(new { uploaded }, "הקבצים הועלו בהצלחה"));
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

    private Guid? GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var guid) ? guid : null;
    }
}
