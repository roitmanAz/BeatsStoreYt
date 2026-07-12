using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Security;

public class SystemLogConfiguration : IEntityTypeConfiguration<SystemLog>
{
    public void Configure(EntityTypeBuilder<SystemLog> builder)
    {
        builder.ToTable("SystemLogs");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Level)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(l => l.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Message)
            .IsRequired();

        builder.Property(l => l.ExceptionJson)
            .IsRequired(false);

        builder.Property(l => l.CorrelationId)
            .HasMaxLength(100);

        builder.Property(l => l.CreatedAt)
            .IsRequired();

        builder.HasIndex(l => l.Level);
        builder.HasIndex(l => l.Category);
        builder.HasIndex(l => l.CreatedAt);
    }
}
