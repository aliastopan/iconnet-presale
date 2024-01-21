using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using IConnet.Presale.Application;
using IConnet.Presale.Infrastructure;
using IConnet.Presale.WebApp.Components;
using IConnet.Presale.WebApp.Logging;
using IConnet.Presale.WebApp.Security;
using IConnet.Presale.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.Host.ConfigureServices((context, services) =>
{
    services.AddApplicationServices(ServiceScope.WEBAPP_ONLY_SERVICE);
    services.AddInfrastructureServices(ServiceScope.WEBAPP_ONLY_SERVICE, context);
    services.AddHttpContextAccessor();
    services.AddAccessControl();
    services.AddHttpClient<IdentityClientService>((_, httpClient) =>
    {
        httpClient.BaseAddress = new Uri("https://localhost:7244");
    });
    services.AddScoped<SessionService>();
    services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

    services.AddScoped<CrmImportService>();

    services.AddRazorComponents()
            .AddInteractiveServerComponents();
    services.AddFluentUIComponents();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
