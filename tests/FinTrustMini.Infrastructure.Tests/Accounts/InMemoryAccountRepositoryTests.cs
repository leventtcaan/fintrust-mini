using FinTrustMini.Domain.Accounts;
using FinTrustMini.Infrastructure.Accounts;

namespace FinTrustMini.Infrastructure.Tests.Accounts;

public sealed class InMemoryAccountRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldStoreAccount()
    {
        var repository = new InMemoryAccountRepository();
        var account = CreateAccount();

        await repository.AddAsync(account, CancellationToken.None);

        var storedAccount = await repository.GetByIdAsync(account.Id, CancellationToken.None);

        Assert.Same(account, storedAccount);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenAccountDoesNotExist()
    {
        var repository = new InMemoryAccountRepository();

        var storedAccount = await repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(storedAccount);
    }

    private static Account CreateAccount()
    {
        return new Account(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "TR000000000000000000000001",
            100m);
    }
}
