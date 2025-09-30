using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Alerto.API.ToDo.Services;
using Alerto.Domain.Interfaces;
using Alerto.Infrastructure.Services;
using Alerto.Infrastructure.Services.ModoChatoService;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.AspNetCore;

namespace Alerto.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInsfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<IAlertoService, AlertoService>();
        services.AddScoped<ISMSSender, SMSSender>();
        //services.AddHostedService<NotificacaoTaskService>();

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = configuration["GoogleAuth:ClientId"];
                options.ClientSecret = configuration["GoogleAuth:ClientSecret"];
                //options.CallbackPath = "/api/conta/autenticar-google";
            })
            .AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JWT:issuer"],
                    ValidAudience = configuration["JWT:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(configuration["JWT:key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
                op.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Token inválido: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token válido");
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddScoped<ICurrentUser, CurrentUserService>();


        services.AddQuartz(q =>
        {
            q.UsePersistentStore(options =>
            {
                options.UseMySql(configuration.GetConnectionString("Default"));
                options.UseNewtonsoftJsonSerializer();
            });
        });

        // Inicia o serviço junto com o App
        services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });


        return services;
    }
}