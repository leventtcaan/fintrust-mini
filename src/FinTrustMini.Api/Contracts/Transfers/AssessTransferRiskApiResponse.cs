namespace FinTrustMini.Api.Contracts.Transfers;

public sealed record AssessTransferRiskApiResponse(
    bool IsAllowed,
    string RiskLevel,
    int RiskScore,
    IReadOnlyCollection<string> Reasons);
