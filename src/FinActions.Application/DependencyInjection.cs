using FinActions.Domain.AppSettings;
using Microsoft.Extensions.DependencyInjection;

namespace FinActions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        ConnectionStrings connectionStrings)
    {
        services.AddInfrastructureServices(connectionStrings);
        services.AddTransient<Contracts.Usuario.IUsuarioService, Application.Usuarios.UsuarioService>();
        services.AddTransient<Application.Token.ITokenService, Application.Token.TokenService>();

        return services;
    }
}