using System.Collections.Concurrent;
using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Accounts;

namespace FinTrustMini.Infrastructure.Accounts;

public sealed class InMemoryAccountRepository : IAccountRepository
{
    private readonly ConcurrentDictionary<Guid, Account> _accounts = new();

    public Task AddAsync(Account account, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _accounts[account.Id] = account;

        return Task.CompletedTask;
    }

    public Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _accounts[account.Id] = account;

        return Task.CompletedTask;
    }

    public Task<Account?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _accounts.TryGetValue(accountId, out var account);

        return Task.FromResult(account);
    }
}
