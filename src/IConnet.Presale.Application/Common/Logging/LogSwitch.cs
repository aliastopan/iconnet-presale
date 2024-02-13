using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace IConnet.Presale.Application.Common.Logging;

public static class LogSwitch
{
    public static LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch();

    public static void Debug(string messageTemplate, params object[] propertyValues)
    {
        var revertLevel = LevelSwitch.MinimumLevel;

        LevelSwitch.MinimumLevel = LogEventLevel.Debug;
        Log.Debug(messageTemplate, propertyValues);
        LevelSwitch.MinimumLevel = revertLevel;
    }
}
