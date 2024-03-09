using System.Reflection;
using IConnet.Presale.Application;
using IConnet.Presale.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.Host.ConfigureServices((context, services) =>
{
    services.AddApplicationServices(ServiceScope.API_ONLY_SERVICE);
    services.AddInfrastructureServices(ServiceScope.API_ONLY_SERVICE, context);
    services.AddEndpointDefinitions(Assembly.GetExecutingAssembly());
    services.AddSecurityTokenAuthentication();
    services.AddSecurityTokenAuthorization();

    services.AddHostedService<RefreshTokenCollectorService>();
});

var app = builder.Build();

// app.InitializeDbContext();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseEndpointDefinitions();

app.UseAuthentication();
app.UseAuthorization();

app.Run();