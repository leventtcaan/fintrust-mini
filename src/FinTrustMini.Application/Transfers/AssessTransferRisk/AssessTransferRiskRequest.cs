namespace FinTrustMini.Application.Transfers.AssessTransferRisk;

public sealed record AssessTransferRiskRequest(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Description);
