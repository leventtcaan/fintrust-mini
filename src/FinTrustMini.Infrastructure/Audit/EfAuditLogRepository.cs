using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Audit;
using FinTrustMini.Infrastructure.Persistence;
using FinTrustMini.Infrastructure.Persistence.Models;

namespace FinTrustMini.Infrastructure.Audit;

public sealed class EfAuditLogRepository : IAuditLogRepository
{
    private readonly FinTrustMiniDbContext _dbContext;

    public EfAuditLogRepository(FinTrustMiniDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken)
    {
        _dbContext.AuditLogs.Add(AuditLogRecord.FromDomain(auditLog));

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
