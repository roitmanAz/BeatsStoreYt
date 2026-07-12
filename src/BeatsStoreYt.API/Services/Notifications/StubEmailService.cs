using Microsoft.Extensions.Logging;

namespace BeatsStoreYt.API.Services.Notifications;

public class StubEmailService : IEmailService
{
    private readonly ILogger<StubEmailService> _logger;

    public StubEmailService(ILogger<StubEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string toEmail, string subject, string body, CancellationToken ct = default)
    {
        _logger.LogInformation("Stub email sent. To={ToEmail}, Subject={Subject}, Body={Body}", toEmail, subject, body);
        return Task.CompletedTask;
    }
}
