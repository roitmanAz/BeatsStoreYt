namespace BeatsStoreYt.API.Data.Features.Content;

// Stores uploaded media metadata and the Blob Storage key instead of a permanent public file URL.
// Used for videos, audio previews, and images so the app can generate secure links on demand.
public class MediaAsset
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string MediaType { get; set; } = string.Empty;

    public string BlobStorageKey { get; set; } = string.Empty;

    public string? PublicPreviewUrl { get; set; }

    public string? ThumbnailUrl { get; set; }

    public int? DurationSeconds { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // This row stores the Blob key, not the file itself, so links can be generated securely later.
}
