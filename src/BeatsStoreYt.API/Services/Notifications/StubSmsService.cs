using Microsoft.Extensions.Logging;

namespace BeatsStoreYt.API.Services.Notifications;

public class StubSmsService : ISmsService
{
    private readonly ILogger<StubSmsService> _logger;

    public StubSmsService(ILogger<StubSmsService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string toPhoneNumber, string message, CancellationToken ct = default)
    {
        _logger.LogInformation("Stub SMS sent. To={ToPhoneNumber}, Message={Message}", toPhoneNumber, message);
        return Task.CompletedTask;
    }
}
