namespace BeatsStoreYt.API.Services.Admin;

public interface IAuditLogService
{
    Task WriteAsync(
        Guid? userId,
        string action,
        string entityName,
        string entityId,
        object? oldValues,
        object? newValues,
        string? ip,
        CancellationToken ct = default);
}
