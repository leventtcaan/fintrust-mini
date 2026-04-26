using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Accounts;

namespace FinTrustMini.Application.Accounts.CreateAccount;

public sealed class CreateAccountService
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<CreateAccountResult> CreateAsync(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var account = new Account(
            Guid.NewGuid(),
            request.CustomerId,
            request.Iban,
            request.OpeningBalance);

        await _accountRepository.AddAsync(account, cancellationToken);

        return new CreateAccountResult(
            account.Id,
            account.CustomerId,
            account.Iban,
            account.Balance);
    }
}
