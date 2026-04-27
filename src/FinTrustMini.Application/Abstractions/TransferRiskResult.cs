namespace FinTrustMini.Application.Abstractions;

public sealed record TransferRiskResult(bool IsAllowed, string? Reason)
{
    public static TransferRiskResult Allowed()
    {
        return new TransferRiskResult(true, null);
    }

    public static TransferRiskResult Rejected(string reason)
    {
        return new TransferRiskResult(false, reason);
    }
}
