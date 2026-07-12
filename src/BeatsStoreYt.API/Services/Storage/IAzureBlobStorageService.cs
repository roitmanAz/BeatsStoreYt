namespace BeatsStoreYt.API.Services.Storage;

public interface IAzureBlobStorageService
{
    Task<bool> ExistsAsync(string blobPath, CancellationToken ct = default);

    Task<string> UploadFileAsync(BlobFileUploadRequest request, CancellationToken ct = default);

    Task DeleteFileAsync(string blobPath, CancellationToken ct = default);

    Task<BlobDeleteResult> DeleteFolderAsync(string folderPath, CancellationToken ct = default);

    Task<string> CreateFolderAsync(string folderPath, CancellationToken ct = default);

    Task<List<BlobFileInfo>> ListFilesAsync(string? folderPath = null, CancellationToken ct = default);

    Task<BlobListResult> ListTreeAsync(string? folderPath = null, CancellationToken ct = default);

    Task<string> GetReadUrlAsync(string blobPath, TimeSpan validFor, CancellationToken ct = default);
}
