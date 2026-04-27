using FinTrustMini.Domain.Transfers;

namespace FinTrustMini.Application.Abstractions;

public interface ITransferRepository
{
    Task AddAsync(Transfer transfer, CancellationToken cancellationToken);

    Task<Transfer?> GetByIdAsync(Guid transferId, CancellationToken cancellationToken);
}
