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
            Log.Warning("Infrastructure:API-ONLY SERVICE");
            services.ConfigureApiServices(context.Configuration, context.HostingEnvironment);
        }

        if (scope is ServiceScope.WEBAPP_ONLY_SERVICE)
        {
            Log.Warning("Infrastructure:WEBAPP-ONLY SERVICE");
            services.ConfigureWebAppServices(context.Configuration);
        }

        Log.Warning("Infrastructure:COMMON SERVICE");
        services.Configure<AppSecretSettings>(context.Configuration.GetSection(AppSecretSettings.SectionName));

        services.AddSingleton<ISecurityTokenValidatorService, SecurityTokenValidatorProvider>();
        services.AddSingleton<IDateTimeService, DateTimeProvider>();
        services.AddSingleton<IPasswordService, PasswordProvider>();
        // TODO: Replace password service with Bcrypt
        // services.AddSingleton<IPasswordService, BcryptPasswordService>();

        services.AddScoped<IAccessTokenService, AccessTokenProvider>();

        return services;
    }

    internal static IServiceCollection ConfigureApiServices(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        services.ConfigureDbContext(configuration, environment);

        services.AddScoped<IIdentityAggregateHandler, IdentityAggregateHandler>();
        services.AddScoped<IIdentityManager, IdentityManager>();
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        services.AddScoped<IRefreshTokenService, RefreshTokenProvider>();

        return services;
    }

    internal static IServiceCollection ConfigureWebAppServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureHttpClient(configuration);

        services.AddSingleton<WorkloadFactory>();
        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            var connectionString = configuration[AppSecretSettings.Section.RedisConnection];
            return ConnectionMultiplexer.Connect(connectionString!);
        });
        services.AddSingleton<ICacheService, RedisCacheProvider>();
        services.AddScoped<IWorkloadManager, WorkloadManager>();

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
            var connectionString = configuration[AppSecretSettings.Section.ConnectionString];
            var serverVersion = new MariaDbServerVersion(new Version(10, 6, 16));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, serverVersion);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });
            services.AddScoped<AppDbContextFactory>();
        }

        return services;
    }

    internal static IServiceCollection ConfigureHttpClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IIdentityHttpClientService, IdentityHttpClientProvider>((_, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration["HttpClient:BaseAddress"]!);
        });

        return services;
    }
}
