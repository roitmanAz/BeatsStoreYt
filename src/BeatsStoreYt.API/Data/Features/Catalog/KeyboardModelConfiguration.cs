using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Catalog;

public class KeyboardModelConfiguration : IEntityTypeConfiguration<KeyboardModel>
{
    public void Configure(EntityTypeBuilder<KeyboardModel> builder)
    {
        builder.ToTable("KeyboardModels");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Brand)
            .HasConversion<int>()
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_KeyboardModels_Brand_Enum", "[Brand] IN (1, 2)"));

        builder.HasIndex(m => new { m.Brand, m.Name })
            .IsUnique();

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();
    }
}
