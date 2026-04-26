namespace FinTrustMini.Domain.Accounts;

public sealed class Account
{
    public Account(Guid id, Guid customerId, string iban, decimal openingBalance)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Account id cannot be empty.", nameof(id));
        }

        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("Customer id cannot be empty.", nameof(customerId));
        }

        if (string.IsNullOrWhiteSpace(iban))
        {
            throw new ArgumentException("IBAN is required.", nameof(iban));
        }

        if (openingBalance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(openingBalance), "Opening balance cannot be negative.");
        }

        Id = id;
        CustomerId = customerId;
        Iban = iban;
        Balance = openingBalance;
        Status = AccountStatus.Active;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; }

    public Guid CustomerId { get; }

    public string Iban { get; }

    public decimal Balance { get; private set; }

    public AccountStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; }

    public void Credit(decimal amount)
    {
        EnsureAccountIsActive();

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Credit amount must be greater than zero.");
        }

        Balance += amount;
    }

    public void Debit(decimal amount)
    {
        EnsureAccountIsActive();

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Debit amount must be greater than zero.");
        }

        if (Balance < amount)
        {
            throw new InvalidOperationException("Insufficient balance.");
        }

        Balance -= amount;
    }

    public void Close()
    {
        EnsureAccountIsActive();
        Status = AccountStatus.Closed;
    }

    private void EnsureAccountIsActive()
    {
        if (Status != AccountStatus.Active)
        {
            throw new InvalidOperationException("Account is not active.");
        }
    }
}
