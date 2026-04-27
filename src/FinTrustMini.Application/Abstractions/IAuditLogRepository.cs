using FinTrustMini.Domain.Audit;

namespace FinTrustMini.Application.Abstractions;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken);
}
