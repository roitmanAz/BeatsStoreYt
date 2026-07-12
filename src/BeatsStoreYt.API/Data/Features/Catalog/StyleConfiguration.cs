using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Catalog;

public class StyleConfiguration : IEntityTypeConfiguration<Style>
{
    public void Configure(EntityTypeBuilder<Style> builder)
    {
        builder.ToTable("Styles");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(s => s.Name)
            .IsUnique();

        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        var seedTime = new DateTimeOffset(new DateTime(2026, 1, 1), TimeSpan.Zero);
        builder.HasData(
            new Style { Id = 1, Name = "Freilach", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 2, Name = "Dance", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 3, Name = "Dj", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 4, Name = "Hora", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 5, Name = "Disco", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 6, Name = "March", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 7, Name = "Slow", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 8, Name = "Bass", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 9, Name = "Time3_4", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 10, Name = "Time4_4", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime },
            new Style { Id = 11, Name = "Time6_4", IsActive = true, CreatedAt = seedTime, UpdatedAt = seedTime });
    }
}
