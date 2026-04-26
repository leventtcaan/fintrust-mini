using FinTrustMini.Domain.Accounts;

namespace FinTrustMini.Domain.Tests.Accounts;

public sealed class AccountTests
{
    [Fact]
    public void Constructor_ShouldCreateActiveAccount_WhenInputIsValid()
    {
        var accountId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var account = new Account(accountId, customerId, "TR000000000000000000000001", 100m);

        Assert.Equal(accountId, account.Id);
        Assert.Equal(customerId, account.CustomerId);
        Assert.Equal("TR000000000000000000000001", account.Iban);
        Assert.Equal(100m, account.Balance);
        Assert.Equal(AccountStatus.Active, account.Status);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenOpeningBalanceIsNegative()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Account(Guid.NewGuid(), Guid.NewGuid(), "TR000000000000000000000001", -1m));

        Assert.Equal("openingBalance", exception.ParamName);
    }

    [Fact]
    public void Credit_ShouldIncreaseBalance_WhenAmountIsPositive()
    {
        var account = CreateAccount(openingBalance: 100m);

        account.Credit(50m);

        Assert.Equal(150m, account.Balance);
    }

    [Fact]
    public void Credit_ShouldThrow_WhenAmountIsZero()
    {
        var account = CreateAccount(openingBalance: 100m);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => account.Credit(0m));

        Assert.Equal("amount", exception.ParamName);
    }

    [Fact]
    public void Debit_ShouldDecreaseBalance_WhenBalanceIsEnough()
    {
        var account = CreateAccount(openingBalance: 100m);

        account.Debit(40m);

        Assert.Equal(60m, account.Balance);
    }

    [Fact]
    public void Debit_ShouldThrow_WhenBalanceIsInsufficient()
    {
        var account = CreateAccount(openingBalance: 100m);

        var exception = Assert.Throws<InvalidOperationException>(() => account.Debit(150m));

        Assert.Equal("Insufficient balance.", exception.Message);
        Assert.Equal(100m, account.Balance);
    }

    [Fact]
    public void Debit_ShouldThrow_WhenAccountIsClosed()
    {
        var account = CreateAccount(openingBalance: 100m);
        account.Close();

        var exception = Assert.Throws<InvalidOperationException>(() => account.Debit(10m));

        Assert.Equal("Account is not active.", exception.Message);
        Assert.Equal(100m, account.Balance);
    }

    private static Account CreateAccount(decimal openingBalance)
    {
        return new Account(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "TR000000000000000000000001",
            openingBalance);
    }
}
