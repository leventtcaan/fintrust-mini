namespace FinTrustMini.Api.Contracts.Accounts;

public sealed record CreateAccountApiRequest(
    Guid CustomerId,
    string Iban,
    decimal OpeningBalance);
