using FinTrustMini.Domain.Accounts;

namespace FinTrustMini.Infrastructure.Persistence.Models;

internal sealed class AccountRecord
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public string Iban { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public AccountStatus Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public static AccountRecord FromDomain(Account account)
    {
        return new AccountRecord
        {
            Id = account.Id,
            CustomerId = account.CustomerId,
            Iban = account.Iban,
            Balance = account.Balance,
            Status = account.Status,
            CreatedAtUtc = account.CreatedAtUtc
        };
    }

    public Account ToDomain()
    {
        var account = new Account(Id, CustomerId, Iban, Balance);

        if (Status == AccountStatus.Closed)
        {
            account.Close();
        }

        return account;
    }

    public void UpdateFromDomain(Account account)
    {
        CustomerId = account.CustomerId;
        Iban = account.Iban;
        Balance = account.Balance;
        Status = account.Status;
        CreatedAtUtc = account.CreatedAtUtc;
    }
}
