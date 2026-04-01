using Application.Interfaces;
using Application.Services;
using Infrastructure.Integrations;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IoC;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {
        services.AddSingleton<IGitHubIntegration, GitHubIntegration>();
        services.AddScoped<IRepositorioService, RepositorioService>();

        return services;
    }
}