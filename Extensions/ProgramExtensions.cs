using System.Text.Json;
using DanfolioBackend.Models;
using DanfolioBackend.Repositories;
using DanfolioBackend.Services;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace DanfolioBackend.Extensions;

public static class ProgramExtensions
{
    /// <summary>
    /// Configures all internally sourced services to the dependency injection container
    /// </summary>
    public static void AddInternalServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPortfolioService, PortfolioService>();
    }
    
    /// <summary>
    /// Adds logging configuration to the host container
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
    
        

    /// <summary>
    /// Configures all repositories to the dependency injection container
    /// </summary>
    public static IServiceCollection AddDataRepositories(this IServiceCollection services)
    {
        // Read JSON file at startup
        var workHistories = JsonSerializer.Deserialize<List<WorkHistory>>(File.ReadAllText("Data/work_history.json")) 
                            ?? new List<WorkHistory>();

        // Register as a singleton so it's cached in memory
        services.AddSingleton(workHistories);
        
        /*
        // Access Azure key vault and obtain secret
        var kvUrl = "redacted"; 
        var secretName = "redacted";
        var client = new SecretClient(new Uri(kvUrl), new DefaultAzureCredential());
        KeyVaultSecret secret = client.GetSecret(secretName);
        var connectionString = secret.Value;

        services.AddSingleton(new SqlConnectionFactory(connectionString));
        */
        services.AddScoped<IPortfolioRepository, PortfolioRepository>();

        return services;
    }
    
}