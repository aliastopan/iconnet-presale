using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.SignalR;
using Microsoft.FluentUI.AspNetCore.Components;
using IConnet.Presale.Application;
using IConnet.Presale.Infrastructure;
using IConnet.Presale.WebApp.Components;
using IConnet.Presale.WebApp.Logging;
using IConnet.Presale.WebApp.Security;
using IConnet.Presale.WebApp.Services;
using IConnet.Presale.WebApp.WebSockets;
using IConnet.Presale.WebApp.Managers;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.Host.ConfigureServices((context, services) =>
{
    var configBuilder = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile("presaleapp.json", optional: false, reloadOnChange: true);

    var configuration = configBuilder.Build();
    builder.Services.AddSingleton<IConfiguration>(configuration);

    services.AddTransient(sp => new HttpClient(new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    }));

    services.AddSingleton<AppSettingsService>();
    services.AddSingleton<CircuitHandler, CustomCircuitHandler>();
    services.AddSingleton<CommonDuplicateService>();
    services.AddSingleton<UserManager>();
    services.AddSingleton<ChatTemplateManager>();
    services.AddSingleton<DirectApprovalManager>();
    services.AddSingleton<RepresentativeOfficeManager>();
    services.AddSingleton<RootCauseManager>();
    services.AddSingleton<IntervalCalculatorService>();
    services.AddSingleton<ReportService>();
    services.AddSingleton<WorksheetService>();
    services.AddSingleton<OptionService>();

    services.AddApplicationServices(ServiceScope.WEBAPP_ONLY_SERVICE);
    services.AddInfrastructureServices(ServiceScope.WEBAPP_ONLY_SERVICE, context);
    services.AddHttpContextAccessor();
    services.AddAccessControl();

    services.AddScoped<ChatTemplateService>();
    services.AddScoped<TabNavigationManager>();
    services.AddScoped<SessionService>();
    services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
    services.AddScoped<BroadcastService>();
    services.AddScoped<SqlPushService>();
    services.AddTransient<CrmImportService>();
    services.AddTransient<CsvParserService>();

    services.AddRazorComponents()
            .AddInteractiveServerComponents();
    services.AddFluentUIComponents();

    services.AddHostedService<StartupService>();
    services.AddHostedService<CommonDuplicateCollectorService>();
    services.AddHostedService<SynchronizationService>();
    services.AddSignalR(options =>
    {
        options.EnableDetailedErrors = true;
        options.KeepAliveInterval = TimeSpan.FromSeconds(10);
        options.HandshakeTimeout = TimeSpan.FromSeconds(5);
    });

    Log.Information("HttpClient base address {0}", context.Configuration["HttpClient:BaseAddress"]!);

});

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStatusCodePagesWithRedirects("/redirect/{0}");
// app.UseStatusCodePagesWithReExecute("/redirect/{0}");

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<BroadcastHub>("/broadcast");

app.Run();
