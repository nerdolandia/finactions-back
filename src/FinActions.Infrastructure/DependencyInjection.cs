using FinActions.Domain.AppSettings;
using FinActions.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace FinActions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConnectionStrings connectionStrings)
    {
        services.AddNpgsql<FinActionsContext>(connectionStrings.Default);

        return services;
    }
}