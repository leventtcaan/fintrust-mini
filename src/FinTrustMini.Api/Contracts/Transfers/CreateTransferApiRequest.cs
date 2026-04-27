namespace FinTrustMini.Api.Contracts.Transfers;

public sealed record CreateTransferApiRequest(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Description);
