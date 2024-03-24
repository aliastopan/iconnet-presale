namespace IConnet.Presale.WebApp.Services;

public class IntervalCalculatorService
{
    private TimeOnly _shiftStart;
    private TimeOnly _shiftEnd;

    public IntervalCalculatorService(IConfiguration configuration)
    {
        // _shiftStart = new TimeOnly(8, 0, 0);
        // _shiftEnd = new TimeOnly(21, 0, 0);

        string startShiftString = configuration["WorkingShift:Pagi:Start"]!;
        string endShiftString = configuration["WorkingShift:Malam:End"]!;

        _shiftStart = TimeOnly.ParseExact(startShiftString, "HH:mm:ss", null);
        _shiftEnd = TimeOnly.ParseExact(endShiftString, "HH:mm:ss", null);
    }

    public void ResetShift(TimeOnly shiftStart, TimeOnly shiftEnd)
    {
        _shiftStart = shiftStart;
        _shiftEnd = shiftEnd;
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
        if (startDateTime.TimeOfDay < _shiftStart.ToTimeSpan())
        {
            startDateTime = startDateTime.Date.Add(_shiftStart.ToTimeSpan());
        }
        else if (startDateTime.TimeOfDay >= _shiftEnd.ToTimeSpan())
        {
            startDateTime = startDateTime.Date.AddDays(1).Add(_shiftStart.ToTimeSpan());
        }

        DateTime currentStartDateTime = startDateTime;
        DateTime currentEndDateTime = startDateTime.Date.Add(_shiftEnd.ToTimeSpan());

        while (currentStartDateTime < endDateTime)
        {
            if (currentEndDateTime > endDateTime)
            {
                currentEndDateTime = endDateTime;
            }

            TimeSpan interval = currentEndDateTime - currentStartDateTime;

            if (interval.TotalHours > _shiftEnd.ToTimeSpan().TotalHours)
            {
                interval = _shiftEnd.ToTimeSpan() - currentStartDateTime.TimeOfDay;
            }

            totalInterval += interval;

            currentStartDateTime = currentStartDateTime.Date.AddDays(1).Add(_shiftStart.ToTimeSpan());
            currentEndDateTime = currentStartDateTime.Date.Add(_shiftEnd.ToTimeSpan());
        }

        return totalInterval;
    }
}
