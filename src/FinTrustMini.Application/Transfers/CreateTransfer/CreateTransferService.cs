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
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransferService(
        IAccountRepository accountRepository,
        ITransferRepository transferRepository,
        ITransferRiskPolicy transferRiskPolicy,
        IAuditLogRepository auditLogRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
        _transferRiskPolicy = transferRiskPolicy;
        _auditLogRepository = auditLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTransferResult> CreateAsync(CreateTransferRequest request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async transactionCancellationToken =>
        {
            var transfer = new Transfer(
                Guid.NewGuid(),
                request.FromAccountId,
                request.ToAccountId,
                request.Amount,
                request.Description);

            try
            {
                var fromAccount = await _accountRepository.GetByIdAsync(request.FromAccountId, transactionCancellationToken);
                var toAccount = await _accountRepository.GetByIdAsync(request.ToAccountId, transactionCancellationToken);

                if (fromAccount is null)
                {
                    throw new InvalidOperationException("Source account was not found.");
                }

                if (toAccount is null)
                {
                    throw new InvalidOperationException("Destination account was not found.");
                }

                var riskResult = _transferRiskPolicy.Evaluate(
                    request.FromAccountId,
                    request.ToAccountId,
                    request.Amount,
                    request.Description);

                if (!riskResult.IsAllowed)
                {
                    throw new InvalidOperationException(riskResult.Reason);
                }

                fromAccount.Debit(request.Amount);
                toAccount.Credit(request.Amount);

                await _accountRepository.UpdateAsync(fromAccount, transactionCancellationToken);
                await _accountRepository.UpdateAsync(toAccount, transactionCancellationToken);

                transfer.MarkCompleted();
            }
            catch (Exception exception)
            {
                transfer.MarkFailed(exception.Message);
            }

            await _transferRepository.AddAsync(transfer, transactionCancellationToken);

            var auditLog = new AuditLog(
                Guid.NewGuid(),
                "TransferCreated",
                nameof(Transfer),
                transfer.Id,
                $"Transfer {transfer.Id} finished with status {transfer.Status}.");

            await _auditLogRepository.AddAsync(auditLog, transactionCancellationToken);

            return new CreateTransferResult(
                transfer.Id,
                transfer.FromAccountId,
                transfer.ToAccountId,
                transfer.Amount,
                transfer.Status.ToString(),
                transfer.FailureReason);
        }, cancellationToken);
    }
}
