namespace FinTrustMini.Application.Abstractions;

public interface ITransferRiskPolicy
{
    TransferRiskResult Evaluate(Guid fromAccountId, Guid toAccountId, decimal amount, string description);
}
