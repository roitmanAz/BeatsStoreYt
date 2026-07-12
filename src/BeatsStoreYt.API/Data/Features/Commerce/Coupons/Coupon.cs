using BeatsStoreYt.API.Data.Features.Commerce.Common;

namespace BeatsStoreYt.API.Data.Features.Commerce.Coupons;

// Stores discount codes and their usage limits for promotions and marketing campaigns.
// Used to calculate discounts during checkout and to control coupon eligibility.
public class Coupon
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public DiscountType DiscountType { get; set; }

    public decimal Value { get; set; }

    public int? MaxUses { get; set; }

    public int? MaxUsesPerUser { get; set; }

    public int UsedCount { get; set; }

    public decimal? MinOrderAmount { get; set; }

    public DateTimeOffset? StartsAt { get; set; }

    public DateTimeOffset? ExpiresAt { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    // One coupon can be redeemed many times.
    public ICollection<CouponRedemption> Redemptions { get; set; } = new List<CouponRedemption>();

    // One coupon can be restricted to many products.
    public ICollection<CouponProduct> CouponProducts { get; set; } = new List<CouponProduct>();
}
