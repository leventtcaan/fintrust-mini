using FinTrustMini.Domain.Accounts;

namespace FinTrustMini.Application.Abstractions;

public interface IAccountRepository
{
    Task AddAsync(Account account, CancellationToken cancellationToken);
}
