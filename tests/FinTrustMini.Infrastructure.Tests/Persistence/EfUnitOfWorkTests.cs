using FinTrustMini.Domain.Accounts;
using FinTrustMini.Infrastructure.Accounts;
using FinTrustMini.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FinTrustMini.Infrastructure.Tests.Persistence;

public sealed class EfUnitOfWorkTests
{
    [Fact]
    public async Task ExecuteInTransactionAsync_ShouldCommitChanges_WhenOperationSucceeds()
    {
        await using var database = await CreateDatabaseAsync();
        var unitOfWork = new EfUnitOfWork(database.DbContext);
        var accountRepository = new EfAccountRepository(database.DbContext);
        var account = CreateAccount();

        await unitOfWork.ExecuteInTransactionAsync(
            async cancellationToken =>
            {
                await accountRepository.AddAsync(account, cancellationToken);

                return true;
            },
            CancellationToken.None);

        var storedAccount = await accountRepository.GetByIdAsync(account.Id, CancellationToken.None);

        Assert.NotNull(storedAccount);
    }

    [Fact]
    public async Task ExecuteInTransactionAsync_ShouldRollbackChanges_WhenOperationFails()
    {
        await using var database = await CreateDatabaseAsync();
        var unitOfWork = new EfUnitOfWork(database.DbContext);
        var accountRepository = new EfAccountRepository(database.DbContext);
        var account = CreateAccount();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            unitOfWork.ExecuteInTransactionAsync<bool>(
                async cancellationToken =>
                {
                    await accountRepository.AddAsync(account, cancellationToken);

                    throw new InvalidOperationException("Simulated transaction failure.");
                },
                CancellationToken.None));

        var storedAccount = await accountRepository.GetByIdAsync(account.Id, CancellationToken.None);

        Assert.Null(storedAccount);
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
