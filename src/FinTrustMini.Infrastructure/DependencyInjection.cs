using FinTrustMini.Application.Abstractions;
using FinTrustMini.Application.Transfers.Risk;
using FinTrustMini.Infrastructure.Accounts;
using FinTrustMini.Infrastructure.Audit;
using FinTrustMini.Infrastructure.Transfers;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrustMini.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        services.AddSingleton<ITransferRepository, InMemoryTransferRepository>();
        services.AddSingleton<IAuditLogRepository, InMemoryAuditLogRepository>();
        services.AddSingleton<ITransferRiskPolicy, DefaultTransferRiskPolicy>();

        return services;
    }
}
