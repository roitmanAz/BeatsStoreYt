namespace BeatsStoreYt.API.Data.Features.Content;

// Stores a complete event playlist that can be used for wedding or full-event playback.
// Used as the parent container for ordered media or beat items.
public class EventAudioPlaylist
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid? CoverMediaAssetId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // Optional cover image or preview asset for the playlist.
    // This lets the admin present a playlist with either Blob media or rhythm-product content.
    public MediaAsset? CoverMediaAsset { get; set; }

    // One playlist contains many ordered items.
    // Items can point to beats or media assets, depending on how the full event is built.
    public ICollection<EventAudioPlaylistItem> Items { get; set; } = new List<EventAudioPlaylistItem>();
}
