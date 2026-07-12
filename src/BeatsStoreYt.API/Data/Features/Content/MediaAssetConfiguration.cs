using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Content;

public class MediaAssetConfiguration : IEntityTypeConfiguration<MediaAsset>
{
    public void Configure(EntityTypeBuilder<MediaAsset> builder)
    {
        builder.ToTable("MediaAssets");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.MediaType)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(m => m.BlobStorageKey)
            .IsRequired()
            .HasMaxLength(512);

        builder.HasIndex(m => m.BlobStorageKey)
            .IsUnique();

        builder.Property(m => m.PublicPreviewUrl)
            .HasMaxLength(1000);

        builder.Property(m => m.ThumbnailUrl)
            .HasMaxLength(1000);

        builder.Property(m => m.DurationSeconds)
            .IsRequired(false);

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();
    }
}
