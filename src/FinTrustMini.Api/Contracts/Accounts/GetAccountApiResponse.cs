namespace FinTrustMini.Api.Contracts.Accounts;

public sealed record GetAccountApiResponse(
    Guid AccountId,
    Guid CustomerId,
    string Iban,
    decimal Balance,
    string Status);
