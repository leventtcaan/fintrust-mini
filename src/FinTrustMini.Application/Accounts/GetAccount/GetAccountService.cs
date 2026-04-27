using FinTrustMini.Application.Abstractions;

namespace FinTrustMini.Application.Accounts.GetAccount;

public sealed class GetAccountService
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<GetAccountResult?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

        if (account is null)
        {
            return null;
        }

        return new GetAccountResult(
            account.Id,
            account.CustomerId,
            account.Iban,
            account.Balance,
            account.Status.ToString());
    }
}
