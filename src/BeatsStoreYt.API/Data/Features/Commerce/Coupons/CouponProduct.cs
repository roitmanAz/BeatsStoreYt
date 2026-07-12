using BeatsStoreYt.API.Data.Features.Commerce.Common;

namespace BeatsStoreYt.API.Data.Features.Commerce.Coupons;

// Join table that limits a coupon to specific products only.
// Used when a promotion should apply only to selected beats or beat sets.
public class CouponProduct
{
    public Guid Id { get; set; }

    public Guid CouponId { get; set; }

    public CatalogProductType ProductType { get; set; }

    public int ProductId { get; set; }

    // Many restriction rows belong to one coupon.
    public Coupon Coupon { get; set; } = null!;
}
