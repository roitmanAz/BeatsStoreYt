using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Content;

public class EventAudioPlaylistConfiguration : IEntityTypeConfiguration<EventAudioPlaylist>
{
    public void Configure(EntityTypeBuilder<EventAudioPlaylist> builder)
    {
        builder.ToTable("EventAudioPlaylists");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        // Optional cover asset lets the playlist show a Blob-based thumbnail.
        builder.HasOne(p => p.CoverMediaAsset)
            .WithMany()
            .HasForeignKey("CoverMediaAssetId")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(p => p.IsActive);
    }
}
