using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using IConnet.Presale.Application.Common.Logging;

namespace IConnet.Presale.Api.Logging;

public static class LoggerConfiguration
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        LogSwitch.LevelSwitch.MinimumLevel = LogEventLevel.Information;

        Log.Logger = new Serilog.LoggerConfiguration()
            .MinimumLevel.ControlledBy(LogSwitch.LevelSwitch)
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: CustomConsoleThemes.LiteratePlus)
            .Filter.ByExcluding(logEvent =>
                logEvent.Exception is SecurityTokenExpiredException &&
                logEvent.Level == LogEventLevel.Information)
            .Filter.ByExcluding(logEvent =>
                logEvent.MessageTemplate.Text.Contains("Generated query execution expression"))
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        return builder;
    }
}
