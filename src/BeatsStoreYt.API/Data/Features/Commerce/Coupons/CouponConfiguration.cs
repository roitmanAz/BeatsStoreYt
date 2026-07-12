using BeatsStoreYt.API.Data.Features.Commerce.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Coupons;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.Property(c => c.DiscountType)
            .HasConversion<int>()
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_Coupons_DiscountType_Enum", "[DiscountType] IN (1, 2)"));

        builder.Property(c => c.Value)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.UsedCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(c => c.MinOrderAmount)
            .HasPrecision(18, 2);

        builder.Property(c => c.StartsAt)
            .IsRequired(false);

        builder.Property(c => c.ExpiresAt)
            .IsRequired(false);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        // One coupon can be redeemed many times.
        builder.HasMany(c => c.Redemptions)
            .WithOne(r => r.Coupon)
            .HasForeignKey(r => r.CouponId)
            .OnDelete(DeleteBehavior.Cascade);

        // One coupon can be restricted to many products.
        builder.HasMany(c => c.CouponProducts)
            .WithOne(cp => cp.Coupon)
            .HasForeignKey(cp => cp.CouponId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
