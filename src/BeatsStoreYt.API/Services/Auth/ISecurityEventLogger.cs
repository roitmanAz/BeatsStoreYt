namespace BeatsStoreYt.API.Services.Auth;

public interface ISecurityEventLogger
{
    Task LogAsync(Guid? userId, string eventType, int severity, object? details = null, CancellationToken ct = default);
}
