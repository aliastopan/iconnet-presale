using System.Reflection;

namespace IConnet.Presale.Api.Extensions;

internal static class EndpointExtensions
{
    internal static IServiceCollection AddEndpointDefinitions(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var endpointDefinitions = new List<IEndpointDefinition>();

        foreach (var assembly in assemblies)
        {
            endpointDefinitions.AddRange(assembly.ExportedTypes
                .Where(x => typeof(IEndpointDefinition)
                    .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IEndpointDefinition>());
        }

        foreach (var endpoint in endpointDefinitions)
        {
            if (endpoint is IEndpointService endpointService)
            {
                endpointService.DefineServices(services);
            }
        }

        services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);
        return services;
    }

    internal static WebApplication UseEndpointDefinitions(this WebApplication app)
    {
        var routeEndpoints = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();
        foreach (var route in routeEndpoints)
        {
            route.DefineEndpoints(app);
        }

        return app;
    }
}
