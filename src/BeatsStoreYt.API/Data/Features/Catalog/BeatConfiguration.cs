using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Catalog;

public class BeatConfiguration : IEntityTypeConfiguration<Beat>
{
    public void Configure(EntityTypeBuilder<Beat> builder)
    {
        builder.ToTable("Beats");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(b => b.Title);

        builder.Property(b => b.Description)
            .HasMaxLength(500);

        builder.Property(b => b.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_Beats_Price_Positive", "[Price] > 0"));

        builder.Property(b => b.CoverImageUrl)
            .HasMaxLength(1000);

        builder.Property(b => b.DemoAudioUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(b => b.ProductFileStorageKey)
            .HasMaxLength(512);

        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        builder.Property(b => b.UpdatedAt)
            .IsRequired();

        // One style -> many beats.
        builder.HasOne(b => b.Style)
            .WithMany(s => s.Beats)
            .HasForeignKey(b => b.StyleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // One keyboard model -> many beats.
        builder.HasOne(b => b.KeyboardModel)
            .WithMany(m => m.Beats)
            .HasForeignKey(b => b.KeyboardModelId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(b => new { b.IsActive, b.KeyboardModelId, b.StyleId });
    }
}
