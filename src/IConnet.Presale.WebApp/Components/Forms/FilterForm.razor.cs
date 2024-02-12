namespace IConnet.Presale.WebApp.Components.Forms;

public partial class FilterForm
{
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter] public EventCallback OnFilter { get; set; }

    private readonly WorkloadFilter _filter = new WorkloadFilter();
    public WorkloadFilter Filter => _filter;

    protected override async Task OnInitializedAsync()
    {
        _filter.FilterOffice = SessionService.FilterPreference.KantorPerwakilan;
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

    public IQueryable<WorkPaper>? BaseFilter(IQueryable<WorkPaper>? workPapers)
    {
        // prioritize filter search
        if (Filter.FilterSearch.HasValue())
        {
            // Log.Warning("Filter by Search");
            return workPapers?.Where(x => x.ApprovalOpportunity.IdPermohonan == Filter.FilterSearch);
        }
        else
        {
            if (Filter.IsFilterOfficeSpecified)
            {
                // Log.Warning("Filter by Office");
                return workPapers?.Where(x => x.ApprovalOpportunity.Regional.KantorPerwakilan == Filter.FilterOffice);
            }
            else
            {
                return workPapers;
            }
        }
    }

    public IQueryable<WorkPaper>? FilterWorkPapers(IQueryable<WorkPaper>? workPapers)
    {
        workPapers = BaseFilter(workPapers);

        return workPapers?
            .Where(x => !Filter.IdPermohonan.HasValue() || x.ApprovalOpportunity.IdPermohonan
                .Contains(Filter.IdPermohonan, StringComparison.CurrentCultureIgnoreCase));

    }
}
