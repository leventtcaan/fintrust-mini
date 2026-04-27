using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Audit;
using FinTrustMini.Domain.Accounts;

namespace FinTrustMini.Application.Accounts.CreateAccount;

public sealed class CreateAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuditLogRepository _auditLogRepository;

    public CreateAccountService(IAccountRepository accountRepository, IAuditLogRepository auditLogRepository)
    {
        _accountRepository = accountRepository;
        _auditLogRepository = auditLogRepository;
    }

    public async Task<CreateAccountResult> CreateAsync(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var account = new Account(
            Guid.NewGuid(),
            request.CustomerId,
            request.Iban,
            request.OpeningBalance);

        await _accountRepository.AddAsync(account, cancellationToken);

        var auditLog = new AuditLog(
            Guid.NewGuid(),
            "AccountCreated",
            nameof(Account),
            account.Id,
            $"Account {account.Id} was created for customer {account.CustomerId}.");

        await _auditLogRepository.AddAsync(auditLog, cancellationToken);

        return new CreateAccountResult(
            account.Id,
            account.CustomerId,
            account.Iban,
            account.Balance);
    }
}
