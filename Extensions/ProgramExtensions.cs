using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DanfolioBackend.Repositories;
using DanfolioBackend.Services;
using DanfolioBackend.Utilities;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace DanfolioBackend.Extensions;

public static class ProgramExtensions
{
    /// <summary>
    /// configures all internally sourced services to the dependency injection container
    /// </summary>
    public static void AddInternalServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPortfolioService, PortfolioService>();
    }
    
    /// <summary>
    /// configures all repositories to the dependency injection container
    /// </summary>
    public static IServiceCollection AddDataRepositories(this IServiceCollection services)
    {
        var kvUrl = "https://origin-dev-kv.vault.azure.net/";
        var secretName = "OriginDbConnectionString";
        var client = new SecretClient(new Uri(kvUrl), new DefaultAzureCredential());
        KeyVaultSecret secret = client.GetSecret(secretName);
        var connectionString = secret.Value;

        services.AddSingleton(new SqlConnectionFactory(connectionString));

        services.AddScoped<IPortfolioRepository, PortfolioRepository>();
       
        return services;
    }
    
    /// <summary>
    /// adds logging configuration to the host container
    /// </summary>
    public static void AddLogging(this WebApplicationBuilder builder)
    {

        builder.Host
            .UseSerilog((_, _, loggerConfiguration) =>
            {
                // console logger
                loggerConfiguration
                    .WriteTo
                    .Console(theme: AnsiConsoleTheme.Code);
            });
    }
}