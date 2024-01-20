using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql;

namespace IConnet.Presale.Infrastructure.Persistence;

public static class ConfigurePersistence
{
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
}
