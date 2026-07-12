namespace BeatsStoreYt.API.Services.Notifications;

public interface ISmsService
{
    Task SendAsync(string toPhoneNumber, string message, CancellationToken ct = default);
}
