using FinTrustMini.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrustMini.Infrastructure.Persistence;

public sealed class FinTrustMiniDbContext : DbContext
{
    public FinTrustMiniDbContext(DbContextOptions<FinTrustMiniDbContext> options)
        : base(options)
    {
    }

    internal DbSet<AccountRecord> Accounts => Set<AccountRecord>();

    internal DbSet<TransferRecord> Transfers => Set<TransferRecord>();

    internal DbSet<AuditLogRecord> AuditLogs => Set<AuditLogRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountRecord>(builder =>
        {
            builder.ToTable("Accounts");
            builder.HasKey(account => account.Id);
            builder.Property(account => account.Iban).HasMaxLength(34).IsRequired();
            builder.Property(account => account.Balance).HasPrecision(18, 2);
            builder.Property(account => account.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        });

        modelBuilder.Entity<TransferRecord>(builder =>
        {
            builder.ToTable("Transfers");
            builder.HasKey(transfer => transfer.Id);
            builder.Property(transfer => transfer.Amount).HasPrecision(18, 2);
            builder.Property(transfer => transfer.Description).HasMaxLength(200).IsRequired();
            builder.Property(transfer => transfer.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            builder.Property(transfer => transfer.FailureReason).HasMaxLength(500);
        });

        modelBuilder.Entity<AuditLogRecord>(builder =>
        {
            builder.ToTable("AuditLogs");
            builder.HasKey(auditLog => auditLog.Id);
            builder.Property(auditLog => auditLog.Action).HasMaxLength(100).IsRequired();
            builder.Property(auditLog => auditLog.EntityName).HasMaxLength(100).IsRequired();
            builder.Property(auditLog => auditLog.Details).HasMaxLength(1000).IsRequired();
        });
    }
}
