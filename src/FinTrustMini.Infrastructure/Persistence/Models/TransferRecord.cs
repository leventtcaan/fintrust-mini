using FinTrustMini.Domain.Transfers;

namespace FinTrustMini.Infrastructure.Persistence.Models;

internal sealed class TransferRecord
{
    public Guid Id { get; set; }

    public Guid FromAccountId { get; set; }

    public Guid ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public TransferStatus Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? CompletedAtUtc { get; set; }

    public DateTime? FailedAtUtc { get; set; }

    public string? FailureReason { get; set; }

    public static TransferRecord FromDomain(Transfer transfer)
    {
        return new TransferRecord
        {
            Id = transfer.Id,
            FromAccountId = transfer.FromAccountId,
            ToAccountId = transfer.ToAccountId,
            Amount = transfer.Amount,
            Description = transfer.Description,
            Status = transfer.Status,
            CreatedAtUtc = transfer.CreatedAtUtc,
            CompletedAtUtc = transfer.CompletedAtUtc,
            FailedAtUtc = transfer.FailedAtUtc,
            FailureReason = transfer.FailureReason
        };
    }

    public Transfer ToDomain()
    {
        var transfer = new Transfer(Id, FromAccountId, ToAccountId, Amount, Description);

        if (Status == TransferStatus.Completed)
        {
            transfer.MarkCompleted();
        }

        if (Status == TransferStatus.Failed)
        {
            transfer.MarkFailed(FailureReason ?? "Unknown failure.");
        }

        return transfer;
    }
}
