using BeatsStoreYt.API.Data.Features.Catalog;
using BeatsStoreYt.API.Data.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Analytics;

public class BeatPlayEventConfiguration : IEntityTypeConfiguration<BeatPlayEvent>
{
    public void Configure(EntityTypeBuilder<BeatPlayEvent> builder)
    {
        builder.ToTable("BeatPlayEvents");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.StartedAt)
            .IsRequired();

        builder.Property(e => e.PlayedSeconds)
            .IsRequired();

        builder.Property(e => e.CompletedPercent)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(e => e.Source)
            .HasMaxLength(50);

        builder.Property(e => e.IpHash)
            .HasMaxLength(128);

        // Many play events belong to one beat.
        builder.HasOne(e => e.Beat)
            .WithMany()
            .HasForeignKey(e => e.BeatId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Optional user link allows tracking engagement per customer.
        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.BeatId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.StartedAt);
    }
}
