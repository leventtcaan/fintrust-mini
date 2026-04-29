using FinTrustMini.Application.Abstractions;
using FinTrustMini.Application.Accounts.GetAccount;
using FinTrustMini.Domain.Accounts;

namespace FinTrustMini.Application.Tests.Accounts.GetAccount;

public sealed class GetAccountServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnAccount_WhenAccountExists()
    {
        var account = CreateAccount();
        var repository = new FakeAccountRepository(account);
        var service = new GetAccountService(repository);

        var result = await service.GetByIdAsync(account.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(account.Id, result.AccountId);
        Assert.Equal(account.CustomerId, result.CustomerId);
        Assert.Equal(account.Iban, result.Iban);
        Assert.Equal(account.Balance, result.Balance);
        Assert.Equal(AccountStatus.Active.ToString(), result.Status);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenAccountDoesNotExist()
    {
        var repository = new FakeAccountRepository(account: null);
        var service = new GetAccountService(repository);

        var result = await service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    private static Account CreateAccount()
    {
        return new Account(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "TR000000000000000000000001",
            250m);
    }

    private sealed class FakeAccountRepository : IAccountRepository
    {
        private readonly Account? _account;

        public FakeAccountRepository(Account? account)
        {
            _account = account;
        }

        public Task AddAsync(Account account, CancellationToken cancellationToken)
        {
            throw new NotSupportedException("This fake is only used for account lookup tests.");
        }

        public Task UpdateAsync(Account account, CancellationToken cancellationToken)
        {
            throw new NotSupportedException("This fake is only used for account lookup tests.");
        }

        public Task<Account?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            var account = _account?.Id == accountId ? _account : null;

            return Task.FromResult(account);
        }
    }
}
