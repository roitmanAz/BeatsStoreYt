namespace BeatsStoreYt.API.Services.Storage;

public class BlobListResult
{
    public List<BlobFolderInfo> Folders { get; set; } = [];

    public List<BlobFileInfo> Files { get; set; } = [];
}
