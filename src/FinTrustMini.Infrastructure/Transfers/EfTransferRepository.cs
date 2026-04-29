using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Transfers;
using FinTrustMini.Infrastructure.Persistence;
using FinTrustMini.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrustMini.Infrastructure.Transfers;

public sealed class EfTransferRepository : ITransferRepository
{
    private readonly FinTrustMiniDbContext _dbContext;

    public EfTransferRepository(FinTrustMiniDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Transfer transfer, CancellationToken cancellationToken)
    {
        _dbContext.Transfers.Add(TransferRecord.FromDomain(transfer));

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Transfer?> GetByIdAsync(Guid transferId, CancellationToken cancellationToken)
    {
        var transferRecord = await _dbContext.Transfers
            .AsNoTracking()
            .FirstOrDefaultAsync(transfer => transfer.Id == transferId, cancellationToken);

        return transferRecord?.ToDomain();
    }
}
