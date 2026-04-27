namespace FinTrustMini.Application.Accounts.GetAccount;

public sealed record GetAccountResult(
    Guid AccountId,
    Guid CustomerId,
    string Iban,
    decimal Balance,
    string Status);
