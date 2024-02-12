using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.SystemConsole.Themes;

namespace IConnet.Presale.WebApp.Logging;

public static class LoggerConfiguration
{
    public static LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch();

    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        LevelSwitch.MinimumLevel = LogEventLevel.Information;

        Log.Logger = new Serilog.LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch)
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: CustomConsoleThemes.LiteratePlus)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        return builder;
    }
}
