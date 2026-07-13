using BeatsStoreYt.API.Data.Features.Commerce.Orders;
using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Services;

public class CustomStyleRequest
{
    public int Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public CustomStyleRequestStatus Status { get; set; } = CustomStyleRequestStatus.Pending;

    public string UserUploadUrl { get; set; } = string.Empty;

    public string? AdminProcessedUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Order Order { get; set; } = null!;

    public User User { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
