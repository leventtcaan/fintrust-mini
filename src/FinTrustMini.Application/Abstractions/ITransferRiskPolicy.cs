namespace FinTrustMini.Application.Abstractions;

public interface ITransferRiskPolicy
{
    TransferRiskResult Evaluate(decimal amount);
}
