using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql;
using StackExchange.Redis;

namespace IConnet.Presale.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ServiceScope scope,
        HostBuilderContext context)
    {
        if (scope is ServiceScope.API_ONLY_SERVICE)
        {
            services.ConfigureApiServices(context.Configuration, context.HostingEnvironment);
        }

        if (scope is ServiceScope.WEBAPP_ONLY_SERVICE)
        {
            services.ConfigureWebAppServices(context.Configuration);
        }

        services.Configure<AppSecretSettings>(context.Configuration.GetSection(AppSecretSettings.SectionName));

        services.AddSingleton<WorkPaperFactory>();

        services.AddSingleton<ISecurityTokenValidatorService, SecurityTokenValidatorProvider>();
        services.AddSingleton<IDateTimeService, DateTimeProvider>();
        services.AddSingleton<IPasswordService, PasswordProvider>();

        services.AddScoped<IAccessTokenService, AccessTokenProvider>();

        return services;
    }

    internal static IServiceCollection ConfigureApiServices(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        services.ConfigureDbContext(configuration, environment);

        // aggregate handlers
        services.AddScoped<IIdentityAggregateHandler, IdentityAggregateHandler>();
        services.AddScoped<IWorkPaperAggregateHandler, WorkPaperAggregateHandler>();

        // entity handlers
        services.AddScoped<IChatTemplateHandler, ChatTemplateHandler>();
        services.AddScoped<IRepresentativeOfficeHandler, RepresentativeOfficeHandler>();
        services.AddScoped<IRootCauseHandler, RootCauseHandler>();
        services.AddScoped<IRefreshTokenService, RefreshTokenProvider>();

        // managers
        services.AddScoped<IIdentityManager, IdentityManager>();
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();

        return services;
    }

    internal static IServiceCollection ConfigureWebAppServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureHttpClient(configuration);

        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            var connectionString = configuration[AppSecretSettings.Section.RedisConnectionString];
            var password = configuration[AppSecretSettings.Section.RedisPassword];
            var options = new ConfigurationOptions
            {
                EndPoints = { connectionString! },
                Password = password,
                ConnectTimeout = 5000,
            };
            return ConnectionMultiplexer.Connect(options);
        });
        services.AddSingleton<IOnProgressPersistenceService, RedisProvider>();
        services.AddSingleton<IInMemoryPersistenceService, InMemoryPersistenceProvider>();
        services.AddSingleton<IWorkloadManager, FasterWorkloadManager>();
        services.AddSingleton<IWorkloadForwardingManager, FasterWorkloadManager>();

        return services;
    }

    internal static IServiceCollection ConfigureDbContext(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        if (environment.IsDevelopment() && configuration.UseInMemoryDatabase())
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase($"Database-IConnet.Presale");
                options.UseLazyLoadingProxies();
                options.EnableSensitiveDataLogging();
            });
            services.AddScoped<AppDbContextFactory>();
            services.AddScoped<IDataSeedingService, DataSeedingProvider>();
        }
        else
        {
            string connectionString;

            if (environment.IsProduction())
            {
                connectionString = configuration[AppSecretSettings.Section.MySqlConnectionString]!;
            }
            else
            {
                Log.Warning("Using DEVELOPMENT DB");
                connectionString = configuration[AppSecretSettings.Section.DevelopmentConnectionString]!;
            }

            var serverVersion = new MariaDbServerVersion(new Version(10, 6, 16));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, serverVersion, mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
                options.EnableDetailedErrors();
            });
            services.AddScoped<AppDbContextFactory>();
            services.AddScoped<IDataSeedingService, DataSeedingProvider>();
        }

        return services;
    }

    internal static IServiceCollection ConfigureHttpClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IIdentityHttpClient, IdentityHttpClient>((_, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["HttpClient:BaseAddress"]!);
        });

        services.AddHttpClient<IWorkPaperHttpClient, WorkPaperHttpClient>((_, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["HttpClient:BaseAddress"]!);
        });

        services.AddHttpClient<IChatTemplateHttpClient, ChatTemplateHttpClient>((_, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["HttpClient:BaseAddress"]!);
        });

        services.AddHttpClient<IRepresentativeOfficeHttpClient, RepresentativeOfficeHttpClient>((_, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["HttpClient:BaseAddress"]!);
        });

        services.AddHttpClient<IRootCauseHttpClient, RootCauseHttpClient>((_, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["HttpClient:BaseAddress"]!);
        });

        return services;
    }
}
