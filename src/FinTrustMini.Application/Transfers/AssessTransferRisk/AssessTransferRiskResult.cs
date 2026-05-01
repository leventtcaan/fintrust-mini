namespace FinTrustMini.Application.Transfers.AssessTransferRisk;

public sealed record AssessTransferRiskResult(
    bool IsAllowed,
    string RiskLevel,
    int RiskScore,
    IReadOnlyCollection<string> Reasons);
