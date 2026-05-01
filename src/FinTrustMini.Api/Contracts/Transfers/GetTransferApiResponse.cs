namespace FinTrustMini.Api.Contracts.Transfers;

public sealed record GetTransferApiResponse(
    Guid TransferId,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Description,
    string Status,
    string? FailureReason,
    DateTime CreatedAtUtc,
    DateTime? CompletedAtUtc,
    DateTime? FailedAtUtc);
