using FinTrustMini.Application.Abstractions;

namespace FinTrustMini.Application.Transfers.Risk;

public sealed class DefaultTransferRiskPolicy : ITransferRiskPolicy
{
    public const decimal SingleTransferLimit = 10_000m;

    public TransferRiskResult Evaluate(Guid fromAccountId, Guid toAccountId, decimal amount, string description)
    {
        var riskScore = 0;
        var reasons = new List<string>();
        var hasBlockingRisk = false;

        if (fromAccountId == toAccountId)
        {
            riskScore += 100;
            hasBlockingRisk = true;
            reasons.Add("Source and destination accounts cannot be the same.");
        }

        if (amount > SingleTransferLimit)
        {
            riskScore += 100;
            hasBlockingRisk = true;
            reasons.Add($"Transfer amount exceeds the single transfer limit of {SingleTransferLimit}.");
        }

        if (amount >= 5_000m && amount <= SingleTransferLimit)
        {
            riskScore += 45;
            reasons.Add("Transfer amount is high and should be monitored.");
        }
        else if (amount >= 1_000m && amount <= SingleTransferLimit)
        {
            riskScore += 20;
            reasons.Add("Transfer amount is above the routine low-value threshold.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            riskScore += 15;
            reasons.Add("Transfer description is missing.");
        }
        else if (description.Trim().Length < 5)
        {
            riskScore += 10;
            reasons.Add("Transfer description is too short to explain the payment context.");
        }

        if (reasons.Count == 0)
        {
            reasons.Add("No material risk signal detected.");
        }

        riskScore = Math.Min(riskScore, 100);
        var riskLevel = riskScore >= 70 ? "High" : riskScore >= 30 ? "Medium" : "Low";

        return hasBlockingRisk
            ? TransferRiskResult.Rejected(riskLevel, riskScore, reasons)
            : TransferRiskResult.Allowed(riskLevel, riskScore, reasons);
    }
}
