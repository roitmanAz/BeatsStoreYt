using BeatsStoreYt.API.Data.Features.Catalog;

namespace BeatsStoreYt.API.Data.Features.Content;

// Stores one ordered item inside an event audio playlist.
// Each row can point to either a beat or a media asset depending on the content source.
public class EventAudioPlaylistItem
{
    public Guid Id { get; set; }

    public Guid EventAudioPlaylistId { get; set; }

    public int SortOrder { get; set; }

    public int? BeatId { get; set; }

    public Guid? MediaAssetId { get; set; }

    // Many items belong to one playlist.
    public EventAudioPlaylist EventAudioPlaylist { get; set; } = null!;

    // Optional link to a rhythm product.
    public Beat? Beat { get; set; }

    // Optional link to a stored media asset in Blob.
    public MediaAsset? MediaAsset { get; set; }
}
