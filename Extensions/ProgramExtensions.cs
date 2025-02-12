using System.Text;
using System.Text.Json;
using DanfolioBackend.Models;
using DanfolioBackend.Repositories;
using DanfolioBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        // Note: Since I have a deployed app service, I should use Environment Variables provided by the app
        // to house secrets and connection strings since it's free.  Key Vault has added costs.

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
    
    /// <summary>
    /// Configures JWT Authentication to the host container
    /// </summary>
    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        // Read JWT secret from environment variables in Azure
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
                     ?? throw new InvalidOperationException("JWT_KEY environment variable is missing.");
        
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                        ?? throw new InvalidOperationException("JWT_ISSUER environment variable is missing.");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidIssuer = jwtIssuer
                };
            });

        builder.Services.AddAuthorization();
    }

    /// <summary>
    /// Configures Swagger to the host container
    /// </summary>
    public static void AddJwtSwaggerGen(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' followed by your token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
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
}