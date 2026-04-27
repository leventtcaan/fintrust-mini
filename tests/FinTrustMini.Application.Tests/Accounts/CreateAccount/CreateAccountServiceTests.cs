using FinTrustMini.Application.Abstractions;
using FinTrustMini.Application.Accounts.CreateAccount;
using FinTrustMini.Domain.Accounts;

namespace FinTrustMini.Application.Tests.Accounts.CreateAccount;

public sealed class CreateAccountServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateAccountAndSaveIt_WhenRequestIsValid()
    {
        var repository = new FakeAccountRepository();
        var service = new CreateAccountService(repository);
        var request = new CreateAccountRequest(
            Guid.NewGuid(),
            "TR000000000000000000000001",
            250m);

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.AccountId);
        Assert.Equal(request.CustomerId, result.CustomerId);
        Assert.Equal(request.Iban, result.Iban);
        Assert.Equal(request.OpeningBalance, result.Balance);

        Assert.NotNull(repository.SavedAccount);
        Assert.Equal(result.AccountId, repository.SavedAccount.Id);
        Assert.Equal(request.CustomerId, repository.SavedAccount.CustomerId);
        Assert.Equal(request.Iban, repository.SavedAccount.Iban);
        Assert.Equal(request.OpeningBalance, repository.SavedAccount.Balance);
        Assert.Equal(AccountStatus.Active, repository.SavedAccount.Status);
    }

    [Fact]
    public async Task CreateAsync_ShouldPassCancellationTokenToRepository()
    {
        var repository = new FakeAccountRepository();
        var service = new CreateAccountService(repository);
        var request = new CreateAccountRequest(
            Guid.NewGuid(),
            "TR000000000000000000000001",
            250m);
        using var cancellationTokenSource = new CancellationTokenSource();

        await service.CreateAsync(request, cancellationTokenSource.Token);

        Assert.Equal(cancellationTokenSource.Token, repository.ReceivedCancellationToken);
    }

    private sealed class FakeAccountRepository : IAccountRepository
    {
        public Account? SavedAccount { get; private set; }

        public CancellationToken ReceivedCancellationToken { get; private set; }

        public Task AddAsync(Account account, CancellationToken cancellationToken)
        {
            SavedAccount = account;
            ReceivedCancellationToken = cancellationToken;

            return Task.CompletedTask;
        }

        public Task<Account?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            return Task.FromResult<Account?>(SavedAccount?.Id == accountId ? SavedAccount : null);
        }
    }
}
