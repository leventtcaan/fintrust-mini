using System.Collections.Concurrent;
using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Transfers;

namespace FinTrustMini.Infrastructure.Transfers;

public sealed class InMemoryTransferRepository : ITransferRepository
{
    private readonly ConcurrentDictionary<Guid, Transfer> _transfers = new();

    public Task AddAsync(Transfer transfer, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _transfers[transfer.Id] = transfer;

        return Task.CompletedTask;
    }

    public Task<Transfer?> GetByIdAsync(Guid transferId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _transfers.TryGetValue(transferId, out var transfer);

        return Task.FromResult(transfer);
    }
}
