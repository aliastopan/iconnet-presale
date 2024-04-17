using System.Globalization;

namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class LowerBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    public DateTime? NullableLowerBoundaryDateTime { get; set; }
    public DateTime LowerBoundaryDateTime => NullableLowerBoundaryDateTime!.Value;

    protected override void OnInitialized()
    {
        NullableLowerBoundaryDateTime = SessionService.FilterPreference.LowerBoundaryDateTime;
    }

    protected string GetCurrentLowerBoundaryDate()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return SessionService.FilterPreference.LowerBoundaryDateTime.ToString("dd MMM yyyy", cultureInfo);
    }

    protected void OnLowerBoundaryDateChanged(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableLowerBoundaryDateTime = nullableDateTime.Value;
        SessionService.FilterPreference.LowerBoundaryDateTime = LowerBoundaryDateTime;

        LogSwitch.Debug("Changing lower boundary");
    }
}
