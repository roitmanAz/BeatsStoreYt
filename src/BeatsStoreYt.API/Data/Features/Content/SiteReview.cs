using BeatsStoreYt.API.Data.Features.Users;

namespace BeatsStoreYt.API.Data.Features.Content;

public class SiteReview
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string Content { get; set; } = string.Empty;

    public int Rating { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsApproved { get; set; } = false;

    public DateTimeOffset? ApprovedAt { get; set; }

    public User? User { get; set; }
}
