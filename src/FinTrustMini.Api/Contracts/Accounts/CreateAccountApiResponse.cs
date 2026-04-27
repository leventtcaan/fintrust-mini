namespace FinTrustMini.Api.Contracts.Accounts;

public sealed record CreateAccountApiResponse(
    Guid AccountId,
    Guid CustomerId,
    string Iban,
    decimal Balance);
