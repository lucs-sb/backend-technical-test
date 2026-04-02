using Application.Interfaces;
using Application.Services;
using Infrastructure.Clients.GitHub;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Infrastructure.IoC;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {
        services.AddScoped<IRepositorioService, RepositorioService>();

        return services;
    }

    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
    {
        services.AddSingleton<IRepositorioStore, RepositorioStore>();
        
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        services
            .AddHttpClient<IGitHubHttpClient, GitHubHttpClient>()
            .AddPolicyHandler(retryPolicy);

        return services;
    }
}
