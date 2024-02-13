using Microsoft.Extensions.DependencyInjection;

namespace IConnet.Presale.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, ServiceScope scope)
    {
        if (scope is ServiceScope.API_ONLY_SERVICE)
        {
            LogSwitch.Debug("Application:API-ONLY SERVICE");
            services.AddMediator(options =>
            {
                options.Namespace = "IConnet.Presale.SourceGeneration.Mediator";
                options.ServiceLifetime = ServiceLifetime.Scoped;
            });
        }

        if (scope is ServiceScope.WEBAPP_ONLY_SERVICE)
        {
            LogSwitch.Debug("Application:WEBAPP-ONLY SERVICE");
        }

        return services;
    }
}
