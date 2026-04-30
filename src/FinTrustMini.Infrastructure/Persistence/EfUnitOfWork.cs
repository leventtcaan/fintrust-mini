using FinTrustMini.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FinTrustMini.Infrastructure.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly FinTrustMiniDbContext _dbContext;

    public EfUnitOfWork(FinTrustMiniDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken cancellationToken)
    {
        var executionStrategy = _dbContext.Database.CreateExecutionStrategy();

        return await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var result = await operation(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        });
    }
}
