using FinTrustMini.Application.Abstractions;

namespace FinTrustMini.Application.Transfers.Risk;

public sealed class DefaultTransferRiskPolicy : ITransferRiskPolicy
{
    public const decimal SingleTransferLimit = 10_000m;

    public TransferRiskResult Evaluate(decimal amount)
    {
        if (amount > SingleTransferLimit)
        {
            return TransferRiskResult.Rejected($"Transfer amount exceeds the single transfer limit of {SingleTransferLimit}.");
        }

        return TransferRiskResult.Allowed();
    }
}
