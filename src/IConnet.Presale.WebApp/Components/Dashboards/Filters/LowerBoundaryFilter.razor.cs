namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class LowerBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter] public EventCallback OnLowerBoundaryChanged { get; set; }

    public DateTime? NullableLowerBoundaryDateTime { get; set; }
    public DateTime LowerBoundaryDateTime => NullableLowerBoundaryDateTime!.Value;

    protected override void OnInitialized()
    {
        NullableLowerBoundaryDateTime = SessionService.FilterPreference.LowerBoundaryDateTime;
    }

    protected async Task OnLowerBoundaryDateChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableLowerBoundaryDateTime = nullableDateTime.Value;
        SessionService.FilterPreference.LowerBoundaryDateTime = LowerBoundaryDateTime;

        LogSwitch.Debug("Changing lower boundary");

        if (OnLowerBoundaryChanged.HasDelegate)
        {
            await OnLowerBoundaryChanged.InvokeAsync();
        }
    }
}
