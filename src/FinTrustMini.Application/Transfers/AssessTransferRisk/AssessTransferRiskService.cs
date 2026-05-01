using FinTrustMini.Application.Abstractions;

namespace FinTrustMini.Application.Transfers.AssessTransferRisk;

public sealed class AssessTransferRiskService
{
    private readonly ITransferRiskPolicy _transferRiskPolicy;

    public AssessTransferRiskService(ITransferRiskPolicy transferRiskPolicy)
    {
        _transferRiskPolicy = transferRiskPolicy;
    }

    public AssessTransferRiskResult Assess(AssessTransferRiskRequest request)
    {
        var riskResult = _transferRiskPolicy.Evaluate(
            request.FromAccountId,
            request.ToAccountId,
            request.Amount,
            request.Description);

        return new AssessTransferRiskResult(
            riskResult.IsAllowed,
            riskResult.RiskLevel,
            riskResult.RiskScore,
            riskResult.Reasons);
    }
}
