using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Content;

public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("BlogPosts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Subtitle)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Content)
            .IsRequired();

        builder.Property(p => p.CoverImageUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasIndex(p => p.Slug)
            .IsUnique();

        builder.Property(p => p.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.ViewCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.PublishedAt)
            .IsRequired(false);

        builder.HasIndex(p => new { p.IsPublished, p.PublishedAt });
    }
}
