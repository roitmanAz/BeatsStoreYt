using BeatsStoreYt.API.Data.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Security;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Action)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.EntityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.EntityId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.OldValuesJson)
            .IsRequired(false);

        builder.Property(a => a.NewValuesJson)
            .IsRequired(false);

        builder.Property(a => a.IpHash)
            .HasMaxLength(128);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        // Optional user link helps identify which account made the change.
        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(a => a.EntityName);
        builder.HasIndex(a => a.CreatedAt);
    }
}
