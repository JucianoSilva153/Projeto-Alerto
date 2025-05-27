using Alerto.Application.Services;

namespace Alerto.Application;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ContaService>();
        services.AddScoped<TarefaService>();
        services.AddScoped<NotificacaoService>();
        services.AddScoped<CategoriaService>();
        services.AddScoped<ListaServices>();

        return services;
    }
}