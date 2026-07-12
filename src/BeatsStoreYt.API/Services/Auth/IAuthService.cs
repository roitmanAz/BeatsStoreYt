using BeatsStoreYt.API.DTOs.Auth;

namespace BeatsStoreYt.API.Services.Auth;

public interface IAuthService
{
    Task<AuthResultDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default);

    Task<AuthResultDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default);

    Task ForgotPasswordAsync(ForgotPasswordRequestDto request, string baseUrl, CancellationToken ct = default);

    Task ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken ct = default);
}
