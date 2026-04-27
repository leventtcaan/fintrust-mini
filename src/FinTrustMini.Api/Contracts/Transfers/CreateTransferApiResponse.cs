namespace FinTrustMini.Api.Contracts.Transfers;

public sealed record CreateTransferApiResponse(
    Guid TransferId,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Status,
    string? FailureReason);
