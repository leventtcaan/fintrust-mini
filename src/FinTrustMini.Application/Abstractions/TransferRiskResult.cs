namespace FinTrustMini.Application.Abstractions;

public sealed record TransferRiskResult(
    bool IsAllowed,
    string RiskLevel,
    int RiskScore,
    IReadOnlyCollection<string> Reasons)
{
    public string? Reason => Reasons.FirstOrDefault();

    public static TransferRiskResult Allowed(string riskLevel, int riskScore, IReadOnlyCollection<string> reasons)
    {
        return new TransferRiskResult(true, riskLevel, riskScore, reasons);
    }

    public static TransferRiskResult Rejected(string riskLevel, int riskScore, IReadOnlyCollection<string> reasons)
    {
        return new TransferRiskResult(false, riskLevel, riskScore, reasons);
    }
}
