namespace FinTrustMini.Application.Accounts.CreateAccount;

public sealed record CreateAccountResult(
    Guid AccountId,
    Guid CustomerId,
    string Iban,
    decimal Balance);
