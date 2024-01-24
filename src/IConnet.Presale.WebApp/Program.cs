using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.FluentUI.AspNetCore.Components;
using IConnet.Presale.Application;
using IConnet.Presale.Infrastructure;
using IConnet.Presale.WebApp.Components;
using IConnet.Presale.WebApp.Logging;
using IConnet.Presale.WebApp.Security;
using IConnet.Presale.WebApp.Services;
using IConnet.Presale.WebApp.WebSockets;

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
        httpClient.BaseAddress = new Uri(context.Configuration["HttpClient:BaseAddress"]!);
    });
    services.AddScoped<SessionService>();
    services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
    services.AddScoped<NotificationService>();
    services.AddScoped<CrmImportService>();

    services.AddRazorComponents()
            .AddInteractiveServerComponents();
    services.AddFluentUIComponents();

    services.AddSignalR(options =>
    {
        options.EnableDetailedErrors = true;
        options.KeepAliveInterval = TimeSpan.FromSeconds(10);
        options.HandshakeTimeout = TimeSpan.FromSeconds(5);
    });
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

// app.MapPost("broadcast", async (string message, IHubContext<UpdateHub> context) =>
// {
//     await context.Clients.All.SendAsync("ReceiveUpdate", message);
//     return Results.NoContent();
// });

app.MapHub<NotificationHub>("/broadcast");

app.Run();
