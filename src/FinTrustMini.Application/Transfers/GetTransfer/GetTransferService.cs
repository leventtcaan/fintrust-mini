using FinTrustMini.Application.Abstractions;

namespace FinTrustMini.Application.Transfers.GetTransfer;

public sealed class GetTransferService
{
    private readonly ITransferRepository _transferRepository;

    public GetTransferService(ITransferRepository transferRepository)
    {
        _transferRepository = transferRepository;
    }

    public async Task<GetTransferResult?> GetByIdAsync(Guid transferId, CancellationToken cancellationToken)
    {
        var transfer = await _transferRepository.GetByIdAsync(transferId, cancellationToken);

        if (transfer is null)
        {
            return null;
        }

        return new GetTransferResult(
            transfer.Id,
            transfer.FromAccountId,
            transfer.ToAccountId,
            transfer.Amount,
            transfer.Description,
            transfer.Status.ToString(),
            transfer.FailureReason,
            transfer.CreatedAtUtc,
            transfer.CompletedAtUtc,
            transfer.FailedAtUtc);
    }
}
