namespace FinTrustMini.Domain.Audit;

public sealed class AuditLog
{
    public AuditLog(Guid id, string action, string entityName, Guid entityId, string details)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Audit log id cannot be empty.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(action))
        {
            throw new ArgumentException("Action is required.", nameof(action));
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            throw new ArgumentException("Entity name is required.", nameof(entityName));
        }

        if (entityId == Guid.Empty)
        {
            throw new ArgumentException("Entity id cannot be empty.", nameof(entityId));
        }

        if (string.IsNullOrWhiteSpace(details))
        {
            throw new ArgumentException("Details are required.", nameof(details));
        }

        Id = id;
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
        Details = details;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; }

    public string Action { get; }

    public string EntityName { get; }

    public Guid EntityId { get; }

    public string Details { get; }

    public DateTime CreatedAtUtc { get; }
}
