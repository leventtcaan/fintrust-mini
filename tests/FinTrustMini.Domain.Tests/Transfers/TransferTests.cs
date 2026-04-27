using FinTrustMini.Domain.Transfers;

namespace FinTrustMini.Domain.Tests.Transfers;

public sealed class TransferTests
{
    [Fact]
    public void Constructor_ShouldCreatePendingTransfer_WhenInputIsValid()
    {
        var transferId = Guid.NewGuid();
        var fromAccountId = Guid.NewGuid();
        var toAccountId = Guid.NewGuid();

        var transfer = new Transfer(transferId, fromAccountId, toAccountId, 100m, "Rent payment");

        Assert.Equal(transferId, transfer.Id);
        Assert.Equal(fromAccountId, transfer.FromAccountId);
        Assert.Equal(toAccountId, transfer.ToAccountId);
        Assert.Equal(100m, transfer.Amount);
        Assert.Equal("Rent payment", transfer.Description);
        Assert.Equal(TransferStatus.Pending, transfer.Status);
        Assert.Null(transfer.CompletedAtUtc);
        Assert.Null(transfer.FailedAtUtc);
        Assert.Null(transfer.FailureReason);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenAccountsAreSame()
    {
        var accountId = Guid.NewGuid();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new Transfer(Guid.NewGuid(), accountId, accountId, 100m, "Invalid transfer"));

        Assert.Equal("Source and destination accounts cannot be the same.", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenAmountIsZero()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Transfer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0m, "Invalid transfer"));

        Assert.Equal("amount", exception.ParamName);
    }

    [Fact]
    public void MarkCompleted_ShouldCompleteTransfer_WhenTransferIsPending()
    {
        var transfer = CreateTransfer();

        transfer.MarkCompleted();

        Assert.Equal(TransferStatus.Completed, transfer.Status);
        Assert.NotNull(transfer.CompletedAtUtc);
        Assert.Null(transfer.FailedAtUtc);
        Assert.Null(transfer.FailureReason);
    }

    [Fact]
    public void MarkFailed_ShouldFailTransfer_WhenTransferIsPending()
    {
        var transfer = CreateTransfer();

        transfer.MarkFailed("Insufficient balance.");

        Assert.Equal(TransferStatus.Failed, transfer.Status);
        Assert.NotNull(transfer.FailedAtUtc);
        Assert.Equal("Insufficient balance.", transfer.FailureReason);
        Assert.Null(transfer.CompletedAtUtc);
    }

    [Fact]
    public void MarkFailed_ShouldThrow_WhenReasonIsEmpty()
    {
        var transfer = CreateTransfer();

        var exception = Assert.Throws<ArgumentException>(() => transfer.MarkFailed(""));

        Assert.Equal("reason", exception.ParamName);
    }

    [Fact]
    public void MarkCompleted_ShouldThrow_WhenTransferIsAlreadyCompleted()
    {
        var transfer = CreateTransfer();
        transfer.MarkCompleted();

        var exception = Assert.Throws<InvalidOperationException>(() => transfer.MarkCompleted());

        Assert.Equal("Only pending transfers can be updated.", exception.Message);
    }

    private static Transfer CreateTransfer()
    {
        return new Transfer(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "Test transfer");
    }
}
