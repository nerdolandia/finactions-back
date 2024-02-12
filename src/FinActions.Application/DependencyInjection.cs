using Microsoft.Extensions.DependencyInjection;

namespace FinActions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<Application.Usuario.IUsuarioService, Application.Usuario.UsuarioService>();

        return services;
    }
}