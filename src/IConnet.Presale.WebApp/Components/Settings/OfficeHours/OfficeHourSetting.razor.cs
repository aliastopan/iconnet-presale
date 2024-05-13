namespace IConnet.Presale.WebApp.Components.Settings.OfficeHours;

public partial class OfficeHourSetting : ComponentBase
{
    [Inject] public AppSettingsService AppSettingsService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;

    protected string ShiftStart => AppSettingsService.ShiftStart.ToClock();
    protected string ShiftEnd => AppSettingsService.ShiftEnd.ToClock();

    protected string OfficeHourPagiStart => AppSettingsService.OfficeHourPagiStart.ToClock();
    protected string OfficeHourPagiEnd => AppSettingsService.OfficeHourPagiEnd.ToClock();
    protected string OfficeHourMalamStart => AppSettingsService.OfficeHourMalamStart.ToClock();
    protected string OfficeHourMalamEnd => AppSettingsService.OfficeHourMalamEnd.ToClock();

    protected bool EnableOfficeHourEdit { get; set;}
    protected bool DisableEdit => !EnableOfficeHourEdit;

    protected DateTime? NullableNewOfficeHourPagiStart { get; set; }
    protected DateTime? NullableNewOfficeHourPagiEnd { get; set; }
    protected DateTime? NullableNewOfficeHourMalamStart { get; set; }
    protected DateTime? NullableNewOfficeHourMalamEnd { get; set; }

    protected override void OnInitialized()
    {
        NullableNewOfficeHourPagiStart = AppSettingsService.OfficeHourPagiStart.ToDateTimeFormat();
        NullableNewOfficeHourPagiEnd = AppSettingsService.OfficeHourPagiEnd.ToDateTimeFormat();
        NullableNewOfficeHourMalamStart = AppSettingsService.OfficeHourMalamStart.ToDateTimeFormat();
        NullableNewOfficeHourMalamEnd = AppSettingsService.OfficeHourMalamEnd.ToDateTimeFormat();
    }

    protected void OnNullableNewOfficeHourPagiStartChanged(DateTime? dateTime)
    {
        NullableNewOfficeHourPagiStart = dateTime;

        // LogSwitch.Debug("Change Pagi Start");
    }

    protected void OnNullableNewOfficeHourPagiEndChanged(DateTime? dateTime)
    {
        if (dateTime.ToTimeOnly() < NullableNewOfficeHourPagiStart.ToTimeOnly())
        {
            // LogSwitch.Debug("Invalid early range");
            return;
        }

        NullableNewOfficeHourPagiEnd = dateTime;

        // LogSwitch.Debug("Change Pagi End");
    }

    protected void OnNullableNewOfficeHourMalamStartChanged(DateTime? dateTime)
    {
        NullableNewOfficeHourMalamStart = dateTime;

        // LogSwitch.Debug("Change Malam Start");
    }

    protected void OnNullableNewOfficeHourMalamEndChanged(DateTime? dateTime)
    {
        if (dateTime.ToTimeOnly() < NullableNewOfficeHourMalamStart.ToTimeOnly())
        {
            // LogSwitch.Debug("Invalid early range");
            return;

        }

        NullableNewOfficeHourMalamEnd = dateTime;

        // LogSwitch.Debug("Change Malam End");
    }

    protected string GetPagiTotalHours()
    {
        if (!NullableNewOfficeHourPagiStart.HasValue || !NullableNewOfficeHourPagiEnd.HasValue)
        {
            return "Durasi: 0";
        }

        TimeSpan span = NullableNewOfficeHourPagiEnd.Value - NullableNewOfficeHourPagiStart.Value;
        if (span.TotalHours < 0)
        {
            return "Durasi: 0";
        }

        double totalHours = Math.Round(span.TotalHours, 2);
        int hours = (int)totalHours;
        int minutes = (int)((totalHours - hours) * 60);

        if (minutes == 60)
        {
            hours++;
            minutes = 0;
        }

        return $"Durasi: {hours}.{minutes:D2}";
    }

    protected string GetMalamTotalHours()
    {
        if (!NullableNewOfficeHourMalamStart.HasValue || !NullableNewOfficeHourMalamEnd.HasValue)
        {
            return "Durasi: 0";
        }

        TimeSpan span = NullableNewOfficeHourMalamEnd.Value - NullableNewOfficeHourMalamStart.Value;
        if (span.TotalHours < 0)
        {
            return "Durasi: 0";
        }

        double totalHours = Math.Round(span.TotalHours, 2);
        int hours = (int)totalHours;
        int minutes = (int)((totalHours - hours) * 60);

        if (minutes == 60)
        {
            hours++;
            minutes = 0;
        }

        return $"Durasi: {hours}.{minutes:D2}";
    }

    protected async Task SaveNewPagiOfficeHoursAsync()
    {
        TimeOnly newPagiStart = NullableNewOfficeHourPagiStart.ToTimeOnly();
        TimeOnly newPagiEnd = NullableNewOfficeHourPagiEnd.ToTimeOnly();

        await AppSettingsService.EditOfficeHourPagiAsync(newPagiStart, newPagiEnd);

        EditOfficeHoursToast("Pagi");
    }

    protected async Task SaveNewMalamOfficeHoursAsync()
    {
        TimeOnly newMalamStart = NullableNewOfficeHourMalamStart.ToTimeOnly();
        TimeOnly newMalamEnd = NullableNewOfficeHourMalamEnd.ToTimeOnly();

        await AppSettingsService.EditOfficeHourMalamAsync(newMalamStart, newMalamEnd);

        EditOfficeHoursToast("Malam");
    }

    private void EditOfficeHoursToast(string time)
    {
        var intent = ToastIntent.Success;
        var message = $"Jam kerja {time} telah berhasil diubah.";
        var timeout = 5000;

        ToastService.ShowToast(intent, message, timeout);
    }
}
