namespace BeatsStoreYt.API.Data.Features.Users;

// Stores registered customers and admins with authentication and account status fields.
// Used for login, authorization, profile management, and account lifecycle tracking.
public class User
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public string? PasswordResetTokenHash { get; set; }

    public DateTimeOffset? PasswordResetTokenCreatedAt { get; set; }

    public UserRole Role { get; set; } = UserRole.Customer;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? LastLoginAt { get; set; }

    public DateTimeOffset? LastActiveAt { get; set; }

    public bool IsActive { get; set; } = true;

    public bool EmailConfirmed { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
}
