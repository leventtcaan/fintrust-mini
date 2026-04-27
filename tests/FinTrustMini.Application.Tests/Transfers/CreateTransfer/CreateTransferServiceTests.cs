using FinTrustMini.Application.Abstractions;
using FinTrustMini.Application.Transfers.CreateTransfer;
using FinTrustMini.Domain.Accounts;
using FinTrustMini.Domain.Transfers;

namespace FinTrustMini.Application.Tests.Transfers.CreateTransfer;

public sealed class CreateTransferServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCompleteTransfer_WhenAccountsExistAndBalanceIsEnough()
    {
        var fromAccount = CreateAccount(openingBalance: 500m);
        var toAccount = CreateAccount(openingBalance: 100m);
        var accountRepository = new FakeAccountRepository(fromAccount, toAccount);
        var transferRepository = new FakeTransferRepository();
        var service = new CreateTransferService(accountRepository, transferRepository);
        var request = new CreateTransferRequest(fromAccount.Id, toAccount.Id, 150m, "Invoice payment");

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.Equal("Completed", result.Status);
        Assert.Null(result.FailureReason);
        Assert.Equal(350m, fromAccount.Balance);
        Assert.Equal(250m, toAccount.Balance);

        Assert.NotNull(transferRepository.SavedTransfer);
        Assert.Equal(TransferStatus.Completed, transferRepository.SavedTransfer.Status);
        Assert.Equal(result.TransferId, transferRepository.SavedTransfer.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldFailTransfer_WhenSourceBalanceIsInsufficient()
    {
        var fromAccount = CreateAccount(openingBalance: 50m);
        var toAccount = CreateAccount(openingBalance: 100m);
        var accountRepository = new FakeAccountRepository(fromAccount, toAccount);
        var transferRepository = new FakeTransferRepository();
        var service = new CreateTransferService(accountRepository, transferRepository);
        var request = new CreateTransferRequest(fromAccount.Id, toAccount.Id, 150m, "Invoice payment");

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.Equal("Failed", result.Status);
        Assert.Equal("Insufficient balance.", result.FailureReason);
        Assert.Equal(50m, fromAccount.Balance);
        Assert.Equal(100m, toAccount.Balance);

        Assert.NotNull(transferRepository.SavedTransfer);
        Assert.Equal(TransferStatus.Failed, transferRepository.SavedTransfer.Status);
        Assert.Equal("Insufficient balance.", transferRepository.SavedTransfer.FailureReason);
    }

    [Fact]
    public async Task CreateAsync_ShouldFailTransfer_WhenDestinationAccountDoesNotExist()
    {
        var fromAccount = CreateAccount(openingBalance: 500m);
        var accountRepository = new FakeAccountRepository(fromAccount);
        var transferRepository = new FakeTransferRepository();
        var service = new CreateTransferService(accountRepository, transferRepository);
        var request = new CreateTransferRequest(fromAccount.Id, Guid.NewGuid(), 150m, "Invoice payment");

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.Equal("Failed", result.Status);
        Assert.Equal("Destination account was not found.", result.FailureReason);
        Assert.Equal(500m, fromAccount.Balance);

        Assert.NotNull(transferRepository.SavedTransfer);
        Assert.Equal(TransferStatus.Failed, transferRepository.SavedTransfer.Status);
    }

    private static Account CreateAccount(decimal openingBalance)
    {
        return new Account(
            Guid.NewGuid(),
            Guid.NewGuid(),
            $"TR{Guid.NewGuid():N}"[..26],
            openingBalance);
    }

    private sealed class FakeAccountRepository : IAccountRepository
    {
        private readonly Dictionary<Guid, Account> _accounts;

        public FakeAccountRepository(params Account[] accounts)
        {
            _accounts = accounts.ToDictionary(account => account.Id);
        }

        public Task AddAsync(Account account, CancellationToken cancellationToken)
        {
            _accounts[account.Id] = account;

            return Task.CompletedTask;
        }

        public Task<Account?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            _accounts.TryGetValue(accountId, out var account);

            return Task.FromResult(account);
        }
    }

    private sealed class FakeTransferRepository : ITransferRepository
    {
        public Transfer? SavedTransfer { get; private set; }

        public Task AddAsync(Transfer transfer, CancellationToken cancellationToken)
        {
            SavedTransfer = transfer;

            return Task.CompletedTask;
        }

        public Task<Transfer?> GetByIdAsync(Guid transferId, CancellationToken cancellationToken)
        {
            return Task.FromResult<Transfer?>(SavedTransfer?.Id == transferId ? SavedTransfer : null);
        }
    }
}
