using FinTrustMini.Domain.Audit;

namespace FinTrustMini.Infrastructure.Persistence.Models;

internal sealed class AuditLogRecord
{
    public Guid Id { get; set; }

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    public string Details { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public static AuditLogRecord FromDomain(AuditLog auditLog)
    {
        return new AuditLogRecord
        {
            Id = auditLog.Id,
            Action = auditLog.Action,
            EntityName = auditLog.EntityName,
            EntityId = auditLog.EntityId,
            Details = auditLog.Details,
            CreatedAtUtc = auditLog.CreatedAtUtc
        };
    }
}
