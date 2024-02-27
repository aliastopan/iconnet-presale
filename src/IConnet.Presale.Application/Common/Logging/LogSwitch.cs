using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace IConnet.Presale.Application.Common.Logging;
#pragma warning disable CS0162 // Unreachable code detected

public static class LogSwitch
{
    private const bool IsProduction = false;
    public static LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch();

    public static void Debug(string messageTemplate, params object[] propertyValues)
    {
        if (IsProduction)
        {
            return;
        }

        var revertLevel = LevelSwitch.MinimumLevel;

        LevelSwitch.MinimumLevel = LogEventLevel.Debug;
        Log.Debug(messageTemplate, propertyValues);
        LevelSwitch.MinimumLevel = revertLevel;
    }
}

#pragma warning restore CS0162 // Unreachable code detected
