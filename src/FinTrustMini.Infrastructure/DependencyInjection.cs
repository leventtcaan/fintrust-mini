using FinTrustMini.Application.Abstractions;
using FinTrustMini.Application.Transfers.Risk;
using FinTrustMini.Infrastructure.Accounts;
using FinTrustMini.Infrastructure.Audit;
using FinTrustMini.Infrastructure.Persistence;
using FinTrustMini.Infrastructure.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrustMini.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FinTrustMiniDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IAccountRepository, EfAccountRepository>();
        services.AddScoped<ITransferRepository, EfTransferRepository>();
        services.AddScoped<IAuditLogRepository, EfAuditLogRepository>();
        services.AddSingleton<ITransferRiskPolicy, DefaultTransferRiskPolicy>();

        return services;
    }

    public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FinTrustMiniDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}
