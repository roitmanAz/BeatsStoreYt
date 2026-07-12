using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Content;

public class EventAudioPlaylistItemConfiguration : IEntityTypeConfiguration<EventAudioPlaylistItem>
{
    public void Configure(EntityTypeBuilder<EventAudioPlaylistItem> builder)
    {
        builder.ToTable("EventAudioPlaylistItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.SortOrder)
            .IsRequired();

        // One playlist contains many ordered items.
        builder.HasOne(i => i.EventAudioPlaylist)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.EventAudioPlaylistId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Item can reference a beat when the playlist is built from rhythm products.
        builder.HasOne(i => i.Beat)
            .WithMany()
            .HasForeignKey(i => i.BeatId)
            .OnDelete(DeleteBehavior.SetNull);

        // Item can reference a Blob-backed media asset when it is a recorded event or video.
        builder.HasOne(i => i.MediaAsset)
            .WithMany()
            .HasForeignKey(i => i.MediaAssetId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(i => new { i.EventAudioPlaylistId, i.SortOrder })
            .IsUnique();
    }
}
