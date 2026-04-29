using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Audit;
using FinTrustMini.Domain.Transfers;

namespace FinTrustMini.Application.Transfers.CreateTransfer;

public sealed class CreateTransferService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly ITransferRiskPolicy _transferRiskPolicy;
    private readonly IAuditLogRepository _auditLogRepository;

    public CreateTransferService(
        IAccountRepository accountRepository,
        ITransferRepository transferRepository,
        ITransferRiskPolicy transferRiskPolicy,
        IAuditLogRepository auditLogRepository)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
        _transferRiskPolicy = transferRiskPolicy;
        _auditLogRepository = auditLogRepository;
    }

    public async Task<CreateTransferResult> CreateAsync(CreateTransferRequest request, CancellationToken cancellationToken)
    {
        var transfer = new Transfer(
            Guid.NewGuid(),
            request.FromAccountId,
            request.ToAccountId,
            request.Amount,
            request.Description);

        try
        {
            var fromAccount = await _accountRepository.GetByIdAsync(request.FromAccountId, cancellationToken);
            var toAccount = await _accountRepository.GetByIdAsync(request.ToAccountId, cancellationToken);

            if (fromAccount is null)
            {
                throw new InvalidOperationException("Source account was not found.");
            }

            if (toAccount is null)
            {
                throw new InvalidOperationException("Destination account was not found.");
            }

            var riskResult = _transferRiskPolicy.Evaluate(request.Amount);

            if (!riskResult.IsAllowed)
            {
                throw new InvalidOperationException(riskResult.Reason);
            }

            fromAccount.Debit(request.Amount);
            toAccount.Credit(request.Amount);

            await _accountRepository.UpdateAsync(fromAccount, cancellationToken);
            await _accountRepository.UpdateAsync(toAccount, cancellationToken);

            transfer.MarkCompleted();
        }
        catch (Exception exception)
        {
            transfer.MarkFailed(exception.Message);
        }

        await _transferRepository.AddAsync(transfer, cancellationToken);

        var auditLog = new AuditLog(
            Guid.NewGuid(),
            "TransferCreated",
            nameof(Transfer),
            transfer.Id,
            $"Transfer {transfer.Id} finished with status {transfer.Status}.");

        await _auditLogRepository.AddAsync(auditLog, cancellationToken);

        return new CreateTransferResult(
            transfer.Id,
            transfer.FromAccountId,
            transfer.ToAccountId,
            transfer.Amount,
            transfer.Status.ToString(),
            transfer.FailureReason);
    }
}
