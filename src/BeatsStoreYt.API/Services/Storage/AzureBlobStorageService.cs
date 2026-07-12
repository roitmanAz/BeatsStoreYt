using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;

namespace BeatsStoreYt.API.Services.Storage;

public class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly BlobContainerClient _container;

    public AzureBlobStorageService(IOptions<AzureBlobOptions> options)
    {
        var cfg = options.Value;
        if (string.IsNullOrWhiteSpace(cfg.ConnectionString))
            throw new InvalidOperationException("AzureBlob:ConnectionString is missing");
        if (string.IsNullOrWhiteSpace(cfg.ContainerName))
            throw new InvalidOperationException("AzureBlob:ContainerName is missing");

        var serviceClient = new BlobServiceClient(cfg.ConnectionString);
        _container = serviceClient.GetBlobContainerClient(cfg.ContainerName);
        _container.CreateIfNotExists(PublicAccessType.None);
    }

    public async Task<string> UploadFileAsync(BlobFileUploadRequest request, CancellationToken ct = default)
    {
        var blobPath = NormalizePath(request.BlobPath);
        var blob = _container.GetBlobClient(blobPath);

        var headers = new BlobHttpHeaders
        {
            ContentType = request.ContentType
        };

        await blob.UploadAsync(request.Content, new BlobUploadOptions { HttpHeaders = headers }, ct);
        return blobPath;
    }

    public async Task<bool> ExistsAsync(string blobPath, CancellationToken ct = default)
    {
        var normalized = NormalizePath(blobPath);
        var blob = _container.GetBlobClient(normalized);
        var exists = await blob.ExistsAsync(ct);
        return exists.Value;
    }

    public async Task DeleteFileAsync(string blobPath, CancellationToken ct = default)
    {
        var normalized = NormalizePath(blobPath);
        var blob = _container.GetBlobClient(normalized);
        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: ct);
    }

    public async Task<BlobDeleteResult> DeleteFolderAsync(string folderPath, CancellationToken ct = default)
    {
        var normalizedPrefix = NormalizePath(folderPath).TrimEnd('/') + "/";
        var result = new BlobDeleteResult();

        await foreach (var item in _container.GetBlobsAsync(
            traits: BlobTraits.None,
            states: BlobStates.None,
            prefix: normalizedPrefix,
            cancellationToken: ct))
        {
            var blob = _container.GetBlobClient(item.Name);
            var deleted = await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: ct);
            if (deleted.Value)
                result.DeletedFiles++;
        }

        if (result.DeletedFiles > 0)
            result.DeletedFolders = 1;

        return result;
    }

    public async Task<string> CreateFolderAsync(string folderPath, CancellationToken ct = default)
    {
        var normalized = NormalizePath(folderPath).TrimEnd('/') + "/";
        var placeholder = _container.GetBlobClient(normalized + ".keep");

        await using var stream = new MemoryStream(Array.Empty<byte>());
        await placeholder.UploadAsync(stream, overwrite: true, cancellationToken: ct);

        return normalized;
    }

    public async Task<List<BlobFileInfo>> ListFilesAsync(string? folderPath = null, CancellationToken ct = default)
    {
        var results = new List<BlobFileInfo>();
        var prefix = string.IsNullOrWhiteSpace(folderPath) ? null : NormalizePath(folderPath).TrimEnd('/') + "/";

        await foreach (var item in _container.GetBlobsAsync(
            traits: BlobTraits.Metadata,
            states: BlobStates.None,
            prefix: prefix,
            cancellationToken: ct))
        {
            if (item.Name.EndsWith("/.keep", StringComparison.OrdinalIgnoreCase))
                continue;

            results.Add(new BlobFileInfo
            {
                Name = item.Name,
                Size = item.Properties.ContentLength,
                ContentType = item.Properties.ContentType,
                LastModified = item.Properties.LastModified
            });
        }

        return results;
    }

    public async Task<BlobListResult> ListTreeAsync(string? folderPath = null, CancellationToken ct = default)
    {
        var result = new BlobListResult();
        var prefix = string.IsNullOrWhiteSpace(folderPath) ? null : NormalizePath(folderPath).TrimEnd('/') + "/";
        var seenFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await foreach (var item in _container.GetBlobsAsync(
            traits: BlobTraits.None,
            states: BlobStates.None,
            prefix: prefix,
            cancellationToken: ct))
        {
            var remaining = prefix is null ? item.Name : item.Name[prefix.Length..];
            if (string.IsNullOrWhiteSpace(remaining))
                continue;

            var slashIndex = remaining.IndexOf('/');
            if (slashIndex >= 0)
            {
                var folderName = remaining[..slashIndex];
                if (seenFolders.Add(folderName))
                    result.Folders.Add(new BlobFolderInfo { Name = folderName });

                continue;
            }

            if (item.Name.EndsWith("/.keep", StringComparison.OrdinalIgnoreCase))
                continue;

            result.Files.Add(new BlobFileInfo
            {
                Name = item.Name,
                Size = item.Properties.ContentLength,
                ContentType = item.Properties.ContentType,
                LastModified = item.Properties.LastModified
            });
        }

        result.Folders = result.Folders.OrderBy(f => f.Name).ToList();
        result.Files = result.Files.OrderBy(f => f.Name).ToList();
        return result;
    }

    public Task<string> GetReadUrlAsync(string blobPath, TimeSpan validFor, CancellationToken ct = default)
    {
        var normalized = NormalizePath(blobPath);
        var blob = _container.GetBlobClient(normalized);

        if (!blob.CanGenerateSasUri)
            throw new InvalidOperationException("Cannot generate SAS URL. Ensure storage connection includes account key.");

        var sas = new BlobSasBuilder
        {
            BlobContainerName = _container.Name,
            BlobName = normalized,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(validFor)
        };
        sas.SetPermissions(BlobSasPermissions.Read);

        return Task.FromResult(blob.GenerateSasUri(sas).ToString());
    }

    private static string NormalizePath(string path)
    {
        var normalized = path.Replace('\\', '/').Trim();
        normalized = normalized.TrimStart('/');
        if (string.IsNullOrWhiteSpace(normalized))
            throw new ArgumentException("Path cannot be empty", nameof(path));

        return normalized;
    }
}
