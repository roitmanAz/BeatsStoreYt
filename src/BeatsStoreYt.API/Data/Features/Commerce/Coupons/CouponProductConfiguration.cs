using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatsStoreYt.API.Data.Features.Commerce.Coupons;

public class CouponProductConfiguration : IEntityTypeConfiguration<CouponProduct>
{
    public void Configure(EntityTypeBuilder<CouponProduct> builder)
    {
        builder.ToTable("CouponProducts");

        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.ProductType)
            .HasConversion<int>()
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_CouponProducts_ProductType_Enum", "[ProductType] IN (1, 2)"));

        // Many restriction rows belong to one coupon.
        builder.HasOne(cp => cp.Coupon)
            .WithMany(c => c.CouponProducts)
            .HasForeignKey(cp => cp.CouponId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(cp => new { cp.CouponId, cp.ProductType, cp.ProductId })
            .IsUnique();
    }
}
