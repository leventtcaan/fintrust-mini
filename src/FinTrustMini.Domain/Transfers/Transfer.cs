namespace FinTrustMini.Domain.Transfers;

public sealed class Transfer
{
    public Transfer(Guid id, Guid fromAccountId, Guid toAccountId, decimal amount, string description)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Transfer id cannot be empty.", nameof(id));
        }

        if (fromAccountId == Guid.Empty)
        {
            throw new ArgumentException("Source account id cannot be empty.", nameof(fromAccountId));
        }

        if (toAccountId == Guid.Empty)
        {
            throw new ArgumentException("Destination account id cannot be empty.", nameof(toAccountId));
        }

        if (fromAccountId == toAccountId)
        {
            throw new InvalidOperationException("Source and destination accounts cannot be the same.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Transfer amount must be greater than zero.");
        }

        Id = id;
        FromAccountId = fromAccountId;
        ToAccountId = toAccountId;
        Amount = amount;
        Description = description;
        Status = TransferStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; }

    public Guid FromAccountId { get; }

    public Guid ToAccountId { get; }

    public decimal Amount { get; }

    public string Description { get; }

    public TransferStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; }

    public DateTime? CompletedAtUtc { get; private set; }

    public DateTime? FailedAtUtc { get; private set; }

    public string? FailureReason { get; private set; }

    public void MarkCompleted()
    {
        EnsurePending();

        Status = TransferStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
    }

    public void MarkFailed(string reason)
    {
        EnsurePending();

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Failure reason is required.", nameof(reason));
        }

        Status = TransferStatus.Failed;
        FailedAtUtc = DateTime.UtcNow;
        FailureReason = reason;
    }

    private void EnsurePending()
    {
        if (Status != TransferStatus.Pending)
        {
            throw new InvalidOperationException("Only pending transfers can be updated.");
        }
    }
}
