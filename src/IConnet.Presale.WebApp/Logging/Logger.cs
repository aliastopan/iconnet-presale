using Serilog.Events;

namespace IConnet.Presale.WebApp.Logging;

public static class Logger
{
    public static void Debug(string messageTemplate, params object[] propertyValues)
    {
        LoggerConfiguration.LevelSwitch.MinimumLevel = LogEventLevel.Debug;
        Log.Debug(messageTemplate, propertyValues);
        LoggerConfiguration.LevelSwitch.MinimumLevel = LogEventLevel.Information;
    }
}
