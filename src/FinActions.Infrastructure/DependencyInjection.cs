using FinActions.Domain.AppSettings;
using FinActions.Domain.Categorias;
using FinActions.Infrastructure.Context;
using FinActions.Infrastructure.Repositories.Categorias;
using Microsoft.Extensions.DependencyInjection;

namespace FinActions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConnectionStrings connectionStrings)
    {
        services.AddNpgsql<FinActionsContext>(connectionStrings.Default);
        services.AddTransient<Domain.Usuarios.IUsuarioRepository, Repositories.UsuarioRepository>();
        services.AddTransient<ICategoriaRepository, CategoriaRepository>();

        return services;
    }
}