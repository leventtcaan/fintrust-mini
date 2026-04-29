using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Accounts;
using FinTrustMini.Infrastructure.Persistence;
using FinTrustMini.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrustMini.Infrastructure.Accounts;

public sealed class EfAccountRepository : IAccountRepository
{
    private readonly FinTrustMiniDbContext _dbContext;

    public EfAccountRepository(FinTrustMiniDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken)
    {
        _dbContext.Accounts.Add(AccountRecord.FromDomain(account));

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        var accountRecord = await _dbContext.Accounts.FindAsync([account.Id], cancellationToken);

        if (accountRecord is null)
        {
            _dbContext.Accounts.Add(AccountRecord.FromDomain(account));
        }
        else
        {
            accountRecord.UpdateFromDomain(account);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Account?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var accountRecord = await _dbContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(account => account.Id == accountId, cancellationToken);

        return accountRecord?.ToDomain();
    }
}
