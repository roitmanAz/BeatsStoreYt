using System.Security.Claims;
using BeatsStoreYt.API.Common;
using BeatsStoreYt.API.Services.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeatsStoreYt.API.Controllers;

public abstract class BaseAdminController : ControllerBase
{
    private readonly IAuditLogService _audit;

    protected BaseAdminController(IAuditLogService audit)
    {
        _audit = audit;
    }

    protected Guid? GetActorId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(value, out var id) ? id : null;
    }

    protected string? GetClientIp()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    protected Task WriteAdminAuditAsync(
        string action,
        string entityName,
        string entityId,
        object? oldValues,
        object? newValues,
        CancellationToken ct = default)
    {
        return _audit.WriteAsync(
            GetActorId(),
            action,
            entityName,
            entityId,
            oldValues,
            newValues,
            GetClientIp(),
            ct);
    }

    protected Task LogAdminAction(
        string action,
        string entity,
        int id,
        CancellationToken ct = default)
    {
        return WriteAdminAuditAsync(action, entity, id.ToString(), null, null, ct);
    }

    protected async Task<ActionResult<ApiResponse<object>>?> DeleteEntityAsync<TEntity>(
        DbSet<TEntity> set,
        TEntity? entity,
        string notFoundMessage,
        string action,
        string entityName,
        string entityId,
        DbContext dbContext,
        CancellationToken ct = default)
        where TEntity : class
    {
        if (entity is null)
            return NotFound(ApiResponse<object>.Failure(notFoundMessage));

        set.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await WriteAdminAuditAsync(action, entityName, entityId, entity, null, ct);
        return null;
    }

    protected async Task<ActionResult<ApiResponse<object>>?> SetBooleanStatusAsync<TEntity>(
        TEntity? entity,
        string notFoundMessage,
        Func<TEntity, bool> currentStatus,
        Action<TEntity, bool> applyStatus,
        bool targetStatus,
        DbContext dbContext,
        string action,
        string entityName,
        string entityId,
        Func<TEntity, object?> stateFactory,
        CancellationToken ct = default)
        where TEntity : class
    {
        if (entity is null)
            return NotFound(ApiResponse<object>.Failure(notFoundMessage));

        if (currentStatus(entity) != targetStatus)
        {
            applyStatus(entity, targetStatus);
            await dbContext.SaveChangesAsync(ct);
        }

        await WriteAdminAuditAsync(action, entityName, entityId, null, stateFactory(entity), ct);
        return null;
    }
}
