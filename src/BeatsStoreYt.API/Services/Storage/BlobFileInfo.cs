namespace BeatsStoreYt.API.Services.Storage;

public class BlobFileInfo
{
    public string Name { get; set; } = string.Empty;

    public long? Size { get; set; }

    public string? ContentType { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
