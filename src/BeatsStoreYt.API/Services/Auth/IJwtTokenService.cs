using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.DTOs.Auth;

namespace BeatsStoreYt.API.Services.Auth;

public interface IJwtTokenService
{
    (string Token, DateTimeOffset ExpiresAt) CreateToken(User user);

    AuthUserDto MapToAuthUser(User user);
}
