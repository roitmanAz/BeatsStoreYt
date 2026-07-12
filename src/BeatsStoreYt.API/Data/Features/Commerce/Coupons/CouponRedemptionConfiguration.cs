using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Coupons;

public class CouponRedemptionConfiguration : IEntityTypeConfiguration<CouponRedemption>
{
    public void Configure(EntityTypeBuilder<CouponRedemption> builder)
    {
        builder.ToTable("CouponRedemptions");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.DiscountAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(r => r.RedeemedAt)
            .IsRequired();

        // Many redemptions belong to one coupon.
        builder.HasOne(r => r.Coupon)
            .WithMany(c => c.Redemptions)
            .HasForeignKey(r => r.CouponId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Many redemptions belong to one user.
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Many redemptions belong to one order.
        builder.HasOne(r => r.Order)
            .WithMany()
            .HasForeignKey(r => r.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(r => new { r.CouponId, r.UserId, r.OrderId })
            .IsUnique();
    }
}
