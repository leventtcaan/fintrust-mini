using FinTrustMini.Application.Abstractions;
using FinTrustMini.Application.Transfers.GetTransfer;
using FinTrustMini.Domain.Transfers;

namespace FinTrustMini.Application.Tests.Transfers.GetTransfer;

public sealed class GetTransferServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnTransfer_WhenTransferExists()
    {
        var transfer = CreateCompletedTransfer();
        var repository = new FakeTransferRepository(transfer);
        var service = new GetTransferService(repository);

        var result = await service.GetByIdAsync(transfer.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(transfer.Id, result.TransferId);
        Assert.Equal(transfer.FromAccountId, result.FromAccountId);
        Assert.Equal(transfer.ToAccountId, result.ToAccountId);
        Assert.Equal(transfer.Amount, result.Amount);
        Assert.Equal(transfer.Description, result.Description);
        Assert.Equal(TransferStatus.Completed.ToString(), result.Status);
        Assert.Null(result.FailureReason);
        Assert.NotNull(result.CompletedAtUtc);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTransferDoesNotExist()
    {
        var repository = new FakeTransferRepository(transfer: null);
        var service = new GetTransferService(repository);

        var result = await service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    private static Transfer CreateCompletedTransfer()
    {
        var transfer = new Transfer(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "Get transfer test");

        transfer.MarkCompleted();

        return transfer;
    }

    private sealed class FakeTransferRepository : ITransferRepository
    {
        private readonly Transfer? _transfer;

        public FakeTransferRepository(Transfer? transfer)
        {
            _transfer = transfer;
        }

        public Task AddAsync(Transfer transfer, CancellationToken cancellationToken)
        {
            throw new NotSupportedException("This fake is only used for transfer lookup tests.");
        }

        public Task<Transfer?> GetByIdAsync(Guid transferId, CancellationToken cancellationToken)
        {
            var transfer = _transfer?.Id == transferId ? _transfer : null;

            return Task.FromResult(transfer);
        }
    }
}
