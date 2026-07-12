namespace BeatsStoreYt.API.Services.Storage;

public class BlobFileUploadRequest
{
    public required string BlobPath { get; set; }

    public required string ContentType { get; set; }

    public required Stream Content { get; set; }
}
