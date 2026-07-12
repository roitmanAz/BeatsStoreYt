using BeatsStoreYt.API.Data.Features.Catalog;
using BeatsStoreYt.API.Data.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Analytics;

public class BeatPlayStatsDailyConfiguration : IEntityTypeConfiguration<BeatPlayStatsDaily>
{
    public void Configure(EntityTypeBuilder<BeatPlayStatsDaily> builder)
    {
        builder.ToTable("BeatPlayStatsDaily");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Date)
            .IsRequired();

        builder.Property(s => s.PlayCount)
            .IsRequired();

        builder.Property(s => s.UniqueListeners)
            .IsRequired();

        builder.Property(s => s.TotalPlayedSeconds)
            .IsRequired();

        // Many aggregated rows belong to one beat.
        builder.HasOne(s => s.Beat)
            .WithMany()
            .HasForeignKey(s => s.BeatId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Optional user breakdown for per-customer reporting.
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(s => new { s.Date, s.BeatId, s.UserId })
            .IsUnique();
    }
}
