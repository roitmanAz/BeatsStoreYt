using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Content;

public class WeddingShowcaseConfiguration : IEntityTypeConfiguration<WeddingShowcase>
{
    public void Configure(EntityTypeBuilder<WeddingShowcase> builder)
    {
        builder.ToTable("WeddingShowcases");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Description)
            .HasMaxLength(1000);

        builder.Property(w => w.EventDate)
            .IsRequired(false);

        builder.Property(w => w.IsFeatured)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.UpdatedAt)
            .IsRequired();

        // One showcase uses one Blob-backed media asset.
        builder.HasOne(w => w.MediaAsset)
            .WithMany()
            .HasForeignKey(w => w.MediaAssetId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(w => w.IsFeatured);
        builder.HasIndex(w => w.EventDate);
    }
}
