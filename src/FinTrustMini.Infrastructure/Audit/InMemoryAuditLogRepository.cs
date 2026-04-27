using System.Collections.Concurrent;
using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Audit;

namespace FinTrustMini.Infrastructure.Audit;

public sealed class InMemoryAuditLogRepository : IAuditLogRepository
{
    private readonly ConcurrentBag<AuditLog> _auditLogs = new();

    public IReadOnlyCollection<AuditLog> AuditLogs => _auditLogs.ToArray();

    public Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _auditLogs.Add(auditLog);

        return Task.CompletedTask;
    }
}
