using BeatsStoreYt.API.Data.Features.Commerce.Orders;
using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Commerce.Coupons;

// Stores each successful coupon use so the same code can be tracked per user and order.
// Used for audit history, per-user limits, and discount reporting.
public class CouponRedemption
{
    public Guid Id { get; set; }

    public Guid CouponId { get; set; }

    public Guid UserId { get; set; }

    public Guid OrderId { get; set; }

    public decimal DiscountAmount { get; set; }

    public DateTimeOffset RedeemedAt { get; set; }

    // Many redemptions belong to one coupon.
    public Coupon Coupon { get; set; } = null!;

    // Many redemptions belong to one user.
    public User User { get; set; } = null!;

    // Many redemptions belong to one order.
    public Order Order { get; set; } = null!;
}
