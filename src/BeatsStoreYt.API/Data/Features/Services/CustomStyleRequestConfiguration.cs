using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Services;

public class CustomStyleRequestConfiguration : IEntityTypeConfiguration<CustomStyleRequest>
{
    public void Configure(EntityTypeBuilder<CustomStyleRequest> builder)
    {
        builder.ToTable("CustomStyleRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_CustomStyleRequests_Status_Enum", "[Status] IN (1, 2, 3)"));

        builder.Property(x => x.UserUploadUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.AdminProcessedUrl)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasOne(x => x.Order)
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(x => x.Comments)
            .WithOne(c => c.CustomStyleRequest)
            .HasForeignKey(c => c.CustomStyleRequestId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.OrderId);
        builder.HasIndex(x => x.Status);
    }
}
