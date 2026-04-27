namespace FinTrustMini.Application.Transfers.CreateTransfer;

public sealed record CreateTransferResult(
    Guid TransferId,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Status,
    string? FailureReason);
