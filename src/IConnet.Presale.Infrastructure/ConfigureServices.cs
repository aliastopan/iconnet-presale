using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.ConfigureWebAppServices();
        }

        Log.Warning("Infrastructure:COMMON SERVICE");
        services.Configure<AppSecretSettings>(context.Configuration.GetSection(AppSecretSettings.SectionName));
        services.Configure<SecurityTokenSettings>(context.Configuration.GetSection(SecurityTokenSettings.SectionName));

        services.AddSingleton<ISecurityTokenValidatorService, SecurityTokenValidatorProvider>();
        services.AddSingleton<IDateTimeService, DateTimeProvider>();
        services.AddSingleton<IPasswordService, PasswordProvider>();
        // TODO: Replace password service with Bcrypt
        // services.AddSingleton<IPasswordService, BcryptPasswordService>();

        services.AddScoped<IAccessTokenService, AccessTokenProvider>();
        services.AddScoped<IMailService, MailProvider>();

        return services;
    }

    internal static IServiceCollection ConfigureApiServices(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        services.ConfigureDbContext(configuration, environment);

        services.AddScoped<IIdentityAggregateService, IdentityAggregateProvider>();
        services.AddScoped<IIdentityManager, IdentityManager>();
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        services.AddScoped<IRefreshTokenService, RefreshTokenProvider>();

        return services;
    }

    internal static IServiceCollection ConfigureWebAppServices(this IServiceCollection services)
    {
        return services;
    }
}
