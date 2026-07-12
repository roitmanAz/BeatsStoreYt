using BeatsStoreYt.API.Data;
using BeatsStoreYt.API.Data.Features.Users;
using BeatsStoreYt.API.DTOs.Auth;
using BeatsStoreYt.API.Services.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BeatsStoreYt.API.Services.Auth;

public class AuthService : IAuthService
{
    private readonly BeatsStoreDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;
    private readonly ISecurityEventLogger _securityEventLogger;

    public AuthService(
        BeatsStoreDbContext context,
        PasswordHasher<User> passwordHasher,
        IJwtTokenService jwtTokenService,
        IEmailService emailService,
        ISmsService smsService,
        ISecurityEventLogger securityEventLogger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _emailService = emailService;
        _smsService = smsService;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<AuthResultDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == normalizedEmail, ct);

        if (existingUser is not null)
            throw new InvalidOperationException("מייל כבר קיים במערכת");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = normalizedEmail,
            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim(),
            Role = UserRole.Customer,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            IsActive = true,
            EmailConfirmed = false
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);

        await _securityEventLogger.LogAsync(user.Id, "auth.register.success", 1,
            new { user.Email }, ct);

        var tokenResult = _jwtTokenService.CreateToken(user);
        user.LastLoginAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(ct);

        return new AuthResultDto
        {
            AccessToken = tokenResult.Token,
            ExpiresAt = tokenResult.ExpiresAt,
            User = _jwtTokenService.MapToAuthUser(user)
        };
    }

    public async Task<AuthResultDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == normalizedEmail, ct);

        if (user is null || !user.IsActive)
        {
            await _securityEventLogger.LogAsync(null, "auth.login.failed", 2,
                new { request.Email, reason = "user-not-found-or-inactive" }, ct);
            throw new InvalidOperationException("מייל או סיסמה שגויים");
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verifyResult == PasswordVerificationResult.Failed)
        {
            await _securityEventLogger.LogAsync(user.Id, "auth.login.failed", 2,
                new { user.Email, reason = "invalid-password" }, ct);
            throw new InvalidOperationException("מייל או סיסמה שגויים");
        }

        user.LastLoginAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(ct);

        await _securityEventLogger.LogAsync(user.Id, "auth.login.success", 1,
            new { user.Email }, ct);

        var tokenResult = _jwtTokenService.CreateToken(user);
        return new AuthResultDto
        {
            AccessToken = tokenResult.Token,
            ExpiresAt = tokenResult.ExpiresAt,
            User = _jwtTokenService.MapToAuthUser(user)
        };
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequestDto request, string baseUrl, CancellationToken ct = default)
    {
        var normalizedEmail = request.Email.Trim().ToLower();
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == normalizedEmail && x.IsActive, ct);

        if (user is null)
        {
            await _securityEventLogger.LogAsync(null, "auth.forgot-password.ignored", 1,
                new { request.Email, reason = "user-not-found" }, ct);
            return;
        }

        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        var rawToken = Convert.ToBase64String(tokenBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');

        user.PasswordResetTokenHash = ComputeSha256(rawToken);
        user.PasswordResetTokenCreatedAt = DateTimeOffset.UtcNow;
        user.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(ct);

        var resetUrl = $"{baseUrl.TrimEnd('/')}/reset-password?token={Uri.EscapeDataString(rawToken)}&email={Uri.EscapeDataString(user.Email)}";

        await _emailService.SendAsync(
            user.Email,
            "איפוס סיסמה - BeatsStoreYt",
            $"לחץ על הקישור הבא כדי לאפס סיסמה (תקף 10 דקות): {resetUrl}",
            ct);

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            await _smsService.SendAsync(
                user.PhoneNumber,
                "נשלח אליך מייל לאיפוס סיסמה. הקישור תקף ל-10 דקות.",
                ct);
        }

        await _securityEventLogger.LogAsync(user.Id, "auth.forgot-password.requested", 1,
            new { user.Email }, ct);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken ct = default)
    {
        var tokenHash = ComputeSha256(request.Token);
        var now = DateTimeOffset.UtcNow;

        var user = await _context.Users.FirstOrDefaultAsync(
            x => x.PasswordResetTokenHash == tokenHash
                && x.PasswordResetTokenCreatedAt.HasValue
                && x.PasswordResetTokenCreatedAt.Value.AddMinutes(10) >= now
                && x.IsActive,
            ct);

        if (user is null)
        {
            await _securityEventLogger.LogAsync(null, "auth.reset-password.failed", 2,
                new { reason = "invalid-or-expired-token" }, ct);
            throw new InvalidOperationException("טוקן לא תקין או שפג תוקפו");
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        user.PasswordResetTokenHash = null;
        user.PasswordResetTokenCreatedAt = null;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(ct);

        await _securityEventLogger.LogAsync(user.Id, "auth.reset-password.success", 1,
            new { user.Email }, ct);
    }

    private static string ComputeSha256(string value)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(hash);
    }
}
