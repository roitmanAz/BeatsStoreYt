using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.DTOs.Auth;
using BeatsStoreYt.API.Services.Auth;
using BeatsStoreYt.API.Services.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BeatsStoreYt.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly NotificationOptions _notificationOptions;

    public AuthController(IAuthService authService, IOptions<NotificationOptions> notificationOptions)
    {
        _authService = authService;
        _notificationOptions = notificationOptions.Value;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResultDto>>> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken ct = default)
    {
        var result = await _authService.RegisterAsync(request, ct);
        return Ok(ApiResponse<AuthResultDto>.Success(result, "הרישום בוצע בהצלחה"));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResultDto>>> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken ct = default)
    {
        var result = await _authService.LoginAsync(request, ct);
        return Ok(ApiResponse<AuthResultDto>.Success(result, "התחברת בהצלחה"));
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse<object>>> ForgotPassword(
        [FromBody] ForgotPasswordRequestDto request,
        CancellationToken ct = default)
    {
        await _authService.ForgotPasswordAsync(request, _notificationOptions.FrontendBaseUrl, ct);
        return Ok(ApiResponse<object>.Success(new { }, "אם המייל קיים במערכת נשלח קישור לאיפוס סיסמה"));
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse<object>>> ResetPassword(
        [FromBody] ResetPasswordRequestDto request,
        CancellationToken ct = default)
    {
        await _authService.ResetPasswordAsync(request, ct);
        return Ok(ApiResponse<object>.Success(new { }, "הסיסמה עודכנה בהצלחה"));
    }
}
