using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Catalog;

public class BeatSetConfiguration : IEntityTypeConfiguration<BeatSet>
{
    public void Configure(EntityTypeBuilder<BeatSet> builder)
    {
        builder.ToTable("BeatSets");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(s => s.Name);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_BeatSets_Price_NonNegative", "[Price] >= 0"));

        builder.Property(s => s.CoverImageUrl)
            .HasMaxLength(1000);

        builder.Property(s => s.DemoAudioUrl)
            .HasMaxLength(1000);

        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();
    }
}
