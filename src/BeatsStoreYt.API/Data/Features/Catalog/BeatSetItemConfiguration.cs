using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Catalog;

public class BeatSetItemConfiguration : IEntityTypeConfiguration<BeatSetItem>
{
    public void Configure(EntityTypeBuilder<BeatSetItem> builder)
    {
        builder.ToTable("BeatSetItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.SortOrder)
            .IsRequired()
            .HasDefaultValue(0);

        // One beat set -> many beat-set items.
        builder.HasOne(i => i.BeatSet)
            .WithMany(s => s.BeatSetItems)
            .HasForeignKey(i => i.BeatSetId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // One beat -> many beat-set items.
        builder.HasOne(i => i.Beat)
            .WithMany(b => b.BeatSetItems)
            .HasForeignKey(i => i.BeatId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(i => new { i.BeatSetId, i.BeatId })
            .IsUnique();

        builder.HasIndex(i => new { i.BeatSetId, i.SortOrder });
    }
}
