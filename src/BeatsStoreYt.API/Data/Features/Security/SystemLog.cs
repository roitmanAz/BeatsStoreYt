namespace BeatsStoreYt.API.Data.Features.Security;

// Stores technical application logs for errors and operational troubleshooting.
// Used to investigate system issues, failures, and background process behavior.
public class SystemLog
{
    public Guid Id { get; set; }

    public string Level { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string? ExceptionJson { get; set; }

    public string? CorrelationId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
