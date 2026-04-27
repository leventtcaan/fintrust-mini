namespace FinTrustMini.Application.Transfers.CreateTransfer;

public sealed record CreateTransferRequest(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Description);
