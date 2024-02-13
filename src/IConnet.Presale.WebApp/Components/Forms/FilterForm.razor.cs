namespace IConnet.Presale.WebApp.Components.Forms;

public partial class FilterForm : ComponentBase
{
    [Inject] public SessionService SessionService { get; set; } = default!;
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;

    [Parameter] public EventCallback OnFilter { get; set; }

    private readonly WorkloadFilter _filter = new WorkloadFilter();
    public WorkloadFilter Filter => _filter;
    public string FilterDateTimeRangeLabel => GetDaysRangeLabel();

    protected override void OnInitialized()
    {
        var today = DateTimeService.DateTimeOffsetNow.DateTime;
        Filter.SetFilterDateTimeDefault(today, SessionService.FilterPreference);
    }

    protected override async Task OnInitializedAsync()
    {
        _filter.FilterOffice = SessionService.FilterPreference.KantorPerwakilan;
        _filter.NullableFilterDateTimeMin = SessionService.FilterPreference.TglPermohonanMin;
        _filter.NullableFilterDateTimeMax = SessionService.FilterPreference.TglPermohonanMax;

        await OnFilter.InvokeAsync();
    }

    public async Task FilterByOfficeAsync(string filterOffice)
    {
        Filter.FilterSearch = string.Empty;
        Filter.FilterOffice = filterOffice;
        SessionService.FilterPreference.KantorPerwakilan = filterOffice;

        await OnFilter.InvokeAsync();
    }

    public async Task FilterBySearchAsync(string filterSearch)
    {
        Filter.ResetColumnFilters();
        Filter.FilterOffice = Filter.FilterOfficeDefault;
        SessionService.FilterPreference.KantorPerwakilan = Filter.FilterOffice;
        Filter.FilterSearch = filterSearch;

        await OnFilter.InvokeAsync();
    }

    public async Task SetFilterDateTimeStartAsync(DateTime? nullableDateTime)
    {
        LogSwitch.Debug("DateTime Start {0}", nullableDateTime!);

        if (nullableDateTime is null)
        {
            return;
        }

        var dateTime = nullableDateTime.Value;
        if (dateTime > Filter.NullableFilterDateTimeMax)
        {
            LogSwitch.Debug("DateTime Start cannot be more than DateTime End");
            dateTime = Filter.FilterDateTimeMax.AddDays(-1);
        }

        Filter.NullableFilterDateTimeMin = dateTime;
        SessionService.FilterPreference.TglPermohonanMin = dateTime;

        await OnFilter.InvokeAsync();
    }

    public async Task SetFilterDateTimeEndAsync(DateTime? nullableDateTime)
    {
        LogSwitch.Debug("DateTime End {0}", nullableDateTime!);

        if (nullableDateTime is null)
        {
            return;
        }

        var dateTime = nullableDateTime.Value;
        if (dateTime < Filter.NullableFilterDateTimeMin)
        {
            LogSwitch.Debug("DateTime End cannot be less than DateTime Start");
            dateTime = Filter.FilterDateTimeMin.AddDays(1);
        }

        Filter.NullableFilterDateTimeMax = dateTime;
        SessionService.FilterPreference.TglPermohonanMax = dateTime;

        await OnFilter.InvokeAsync();
    }

    public IQueryable<WorkPaper>? BaseFilter(IQueryable<WorkPaper>? workPapers)
    {
        // prioritize filter search
        if (Filter.FilterSearch.HasValue())
        {
            // LogSwitch.Debug("Filter by Search");
            return workPapers?.Where(x => x.ApprovalOpportunity.IdPermohonan == Filter.FilterSearch);
        }
        else
        {
            if (Filter.IsFilterOfficeSpecified)
            {
                // LogSwitch.Debug("Filter by Office");
                workPapers = workPapers?.Where(x => x.ApprovalOpportunity.Regional.KantorPerwakilan == Filter.FilterOffice);
            }

            LogSwitch.Debug("Filtering DateTime");
            return workPapers?.Where(x => x.ApprovalOpportunity.TglPermohonan >= Filter.FilterDateTimeMin
                        && x.ApprovalOpportunity.TglPermohonan <= Filter.FilterDateTimeMax);
        }
    }

    public IQueryable<WorkPaper>? FilterWorkPapers(IQueryable<WorkPaper>? workPapers)
    {
        workPapers = BaseFilter(workPapers);

        return workPapers?
            .Where(x => !Filter.IdPermohonan.HasValue() || x.ApprovalOpportunity.IdPermohonan
                .Contains(Filter.IdPermohonan, StringComparison.CurrentCultureIgnoreCase));

    }

    private string GetDaysRangeLabel()
    {
        var currentDate = DateTime.Today;
        var isToday = Filter.FilterDateTimeMax.Date == currentDate;
        var denote = isToday ? "Terakhir" : "";

        return $"Rentang {Filter.FilterDateTimeDifference.Days} Hari {denote}";
    }
}
