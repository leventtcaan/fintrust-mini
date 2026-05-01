namespace FinTrustMini.Application.Transfers.GetTransfer;

public sealed record GetTransferResult(
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
