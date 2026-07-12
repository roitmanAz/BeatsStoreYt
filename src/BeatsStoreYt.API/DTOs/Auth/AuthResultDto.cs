namespace BeatsStoreYt.API.DTOs.Auth;

public class AuthResultDto
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }

    public AuthUserDto User { get; set; } = new();
}
