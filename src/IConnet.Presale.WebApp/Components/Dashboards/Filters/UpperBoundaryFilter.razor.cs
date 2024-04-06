namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class UpperBoundaryFilter
{
    [Inject] public SessionService SessionService { get; set; } = default!;

    public DateTime? NullableUpperBoundaryDateTimeMin { get; set; }
    public DateTime? NullableUpperBoundaryDateTimeMax { get; set; }

    protected override void OnInitialized()
    {
        NullableUpperBoundaryDateTimeMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        NullableUpperBoundaryDateTimeMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
    }
}
