namespace IConnet.Presale.WebApp.Components.Settings.OfficeHours;

public partial class OfficeHourSetting
{
    [Inject] public AppSettingsService AppSettingsService { get; set; } = default!;

    protected string ShiftStart => AppSettingsService.ShiftStart.ToClock();
    protected string ShiftEnd => AppSettingsService.ShiftEnd.ToClock();

    protected string OfficeHourPagiStart => AppSettingsService.OfficeHourPagiStart.ToClock();
    protected string OfficeHourPagiEnd => AppSettingsService.OfficeHourPagiEnd.ToClock();
    protected string OfficeHourMalamStart => AppSettingsService.OfficeHourMalamStart.ToClock();
    protected string OfficeHourMalamEnd => AppSettingsService.OfficeHourMalamEnd.ToClock();
}
