using FinTrustMini.Application.Abstractions;
using FinTrustMini.Infrastructure.Accounts;
using FinTrustMini.Infrastructure.Transfers;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrustMini.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        services.AddSingleton<ITransferRepository, InMemoryTransferRepository>();

        return services;
    }
}
