using FinTrustMini.Application.Abstractions;
using FinTrustMini.Domain.Transfers;

namespace FinTrustMini.Application.Transfers.CreateTransfer;

public sealed class CreateTransferService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferRepository _transferRepository;

    public CreateTransferService(IAccountRepository accountRepository, ITransferRepository transferRepository)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
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

            fromAccount.Debit(request.Amount);
            toAccount.Credit(request.Amount);

            transfer.MarkCompleted();
        }
        catch (Exception exception)
        {
            transfer.MarkFailed(exception.Message);
        }

        await _transferRepository.AddAsync(transfer, cancellationToken);

        return new CreateTransferResult(
            transfer.Id,
            transfer.FromAccountId,
            transfer.ToAccountId,
            transfer.Amount,
            transfer.Status.ToString(),
            transfer.FailureReason);
    }
}
