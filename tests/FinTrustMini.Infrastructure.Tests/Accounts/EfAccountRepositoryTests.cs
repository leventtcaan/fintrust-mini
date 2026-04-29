using FinTrustMini.Domain.Accounts;
using FinTrustMini.Infrastructure.Accounts;
using FinTrustMini.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FinTrustMini.Infrastructure.Tests.Accounts;

public sealed class EfAccountRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistAccount()
    {
        await using var database = await CreateDatabaseAsync();
        var repository = new EfAccountRepository(database.DbContext);
        var account = CreateAccount();

        await repository.AddAsync(account, CancellationToken.None);

        var storedAccount = await repository.GetByIdAsync(account.Id, CancellationToken.None);

        Assert.NotNull(storedAccount);
        Assert.Equal(account.Id, storedAccount.Id);
        Assert.Equal(account.CustomerId, storedAccount.CustomerId);
        Assert.Equal(account.Iban, storedAccount.Iban);
        Assert.Equal(account.Balance, storedAccount.Balance);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistBalanceChanges()
    {
        await using var database = await CreateDatabaseAsync();
        var repository = new EfAccountRepository(database.DbContext);
        var account = CreateAccount();

        await repository.AddAsync(account, CancellationToken.None);

        account.Debit(25m);
        await repository.UpdateAsync(account, CancellationToken.None);

        var storedAccount = await repository.GetByIdAsync(account.Id, CancellationToken.None);

        Assert.NotNull(storedAccount);
        Assert.Equal(75m, storedAccount.Balance);
    }

    private static async Task<TestDatabase> CreateDatabaseAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<FinTrustMiniDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbContext = new FinTrustMiniDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();

        return new TestDatabase(connection, dbContext);
    }

    private static Account CreateAccount()
    {
        return new Account(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "TR000000000000000000000001",
            100m);
    }

    private sealed class TestDatabase : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        public TestDatabase(SqliteConnection connection, FinTrustMiniDbContext dbContext)
        {
            _connection = connection;
            DbContext = dbContext;
        }

        public FinTrustMiniDbContext DbContext { get; }

        public async ValueTask DisposeAsync()
        {
            await DbContext.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
