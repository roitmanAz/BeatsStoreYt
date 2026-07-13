using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Content;

public class SiteReviewConfiguration : IEntityTypeConfiguration<SiteReview>
{
    public void Configure(EntityTypeBuilder<SiteReview> builder)
    {
        builder.ToTable("SiteReviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Email)
            .HasMaxLength(256);

        builder.Property(r => r.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(r => r.Rating)
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_SiteReviews_Rating_Range", "[Rating] BETWEEN 1 AND 5"));

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.IsApproved)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(r => r.ApprovedAt)
            .IsRequired(false);

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(r => new { r.IsApproved, r.CreatedAt });
    }
}
