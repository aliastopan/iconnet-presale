namespace IConnet.Presale.WebApp.Services;

public class IntervalCalculatorService
{
    private readonly AppSettingsService _appSettingsService;

    public IntervalCalculatorService(AppSettingsService appSettingsService)
    {
        _appSettingsService = appSettingsService;
    }

    public TimeSpan CalculateInterval(DateTime startDateTime, DateTime endDateTime,
        bool excludeFrozenInterval = true)
    {
        if (!excludeFrozenInterval)
        {
            TimeSpan interval = endDateTime - startDateTime;

            return interval < TimeSpan.Zero
                ? TimeSpan.Zero
                : interval;
        }

        TimeSpan totalInterval = TimeSpan.Zero;

        // handle cases where startDateTime is during the frozen interval
        if (startDateTime.TimeOfDay < _appSettingsService.ShiftStart.ToTimeSpan())
        {
            startDateTime = startDateTime.Date.Add(_appSettingsService.ShiftStart.ToTimeSpan());
        }
        else if (startDateTime.TimeOfDay >= _appSettingsService.ShiftEnd.ToTimeSpan())
        {
            startDateTime = startDateTime.Date.AddDays(1).Add(_appSettingsService.ShiftStart.ToTimeSpan());
        }

        DateTime currentStartDateTime = startDateTime;
        DateTime currentEndDateTime = startDateTime.Date.Add(_appSettingsService.ShiftEnd.ToTimeSpan());

        while (currentStartDateTime < endDateTime)
        {
            if (currentEndDateTime > endDateTime)
            {
                currentEndDateTime = endDateTime;
            }

            TimeSpan interval = currentEndDateTime - currentStartDateTime;

            if (interval.TotalHours > _appSettingsService.ShiftEnd.ToTimeSpan().TotalHours)
            {
                interval = _appSettingsService.ShiftEnd.ToTimeSpan() - currentStartDateTime.TimeOfDay;
            }

            totalInterval += interval;

            currentStartDateTime = currentStartDateTime.Date.AddDays(1).Add(_appSettingsService.ShiftStart.ToTimeSpan());
            currentEndDateTime = currentStartDateTime.Date.Add(_appSettingsService.ShiftEnd.ToTimeSpan());
        }

        return totalInterval;
    }
}
