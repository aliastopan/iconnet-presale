using System.Globalization;

namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class LowerBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    public DateTime CurrentLowerBoundaryDateTime { get; set; }

    public DateTime? NullableLowerBoundaryDateTime { get; set; }
    public DateTime LowerBoundaryDateTime => NullableLowerBoundaryDateTime!.Value;

    protected override void OnInitialized()
    {
        NullableLowerBoundaryDateTime = SessionService.FilterPreference.LowerBoundaryDateTime;

        CurrentLowerBoundaryDateTime = new DateTime(LowerBoundaryDateTime.Ticks);
    }

    protected string GetCurrentLowerBoundaryDate()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return CurrentLowerBoundaryDateTime.ToString("dd MMM yyyy", cultureInfo);
    }

    protected void OnLowerBoundaryDateChanged(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        if (nullableDateTime.Value.Date < SessionService.FilterPreference.UpperBoundaryDateTimeMin.Date)
        {
            LogSwitch.Debug("Invalid lower boundary. Set to upper boundary MIN.");

            NullableLowerBoundaryDateTime = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        }
        else
        {
            LogSwitch.Debug("Changing lower boundary");

            NullableLowerBoundaryDateTime = nullableDateTime.Value;
            SessionService.FilterPreference.LowerBoundaryDateTime = LowerBoundaryDateTime;
        }
    }
}
