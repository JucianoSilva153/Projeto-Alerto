using Alerto.API.ToDo.Repositories.NotificacaoRepository;
using Alerto.Domain.Interfaces;
using Alerto.Persistance.Context;
using Alerto.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Alerto.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DBToDO>(op =>
            op.UseMySql(configuration.GetConnectionString("Default"), ServerVersion.Parse("10.4.32")));

        services.AddScoped<ITarefasRepository, TarefasRepository>();
        services.AddScoped<ICategoriaRepository, CategoriasRepository>();
        services.AddScoped<IListasRepository, ListasRepository>();
        services.AddScoped<IContaRepository, ContasRepository>();
        services.AddScoped<INotificacaoRepository, NotificacaoRepository>();
        
        return services;
    }
}