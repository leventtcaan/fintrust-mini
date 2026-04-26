namespace FinTrustMini.Application.Accounts.CreateAccount;

public sealed record CreateAccountRequest(
    Guid CustomerId,
    string Iban,
    decimal OpeningBalance);
