namespace FinTrustMini.Domain.Customers;

public sealed class Customer
{
    public Customer(Guid id, string fullName, string email)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Customer id cannot be empty.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Full name is required.", nameof(fullName));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        Id = id;
        FullName = fullName;
        Email = email;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; }

    public string FullName { get; }

    public string Email { get; }

    public DateTime CreatedAtUtc { get; }
}
