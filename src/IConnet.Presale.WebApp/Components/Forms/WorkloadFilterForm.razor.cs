namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadFilterForm
{
    [Parameter] public EventCallback OnFilter { get; set; }

    private readonly WorkloadFilter _filter = new WorkloadFilter();
    public WorkloadFilter Filter => _filter;

    public async Task FilterByOfficeAsync(string filterOffice)
    {
        Log.Warning("Filter {0}", filterOffice);
        Filter.FilterOffice = filterOffice;
        Filter.FilterSearch = string.Empty;

        await OnFilter.InvokeAsync();
    }

    public async Task FilterBySearchAsync(string filterSearch)
    {
        Log.Warning("Filter Search {0}", filterSearch);
        Filter.FilterSearch = filterSearch;
        Filter.FilterOffice = Filter.FilterOfficeDefault;

        await OnFilter.InvokeAsync();
    }

    public IQueryable<WorkPaper>? FilterWorkPapers(IQueryable<WorkPaper>? workPapers)
    {
        if (!Filter.IsFilterOfficeSpecified)
        {
            if (Filter.FilterSearch.HasValue())
            {
                Log.Warning("Filter by Search");
                return workPapers?.Where(x => x.ApprovalOpportunity.IdPermohonan == Filter.FilterSearch);
            }
            else
            {
                return workPapers;
            }
        }

        Log.Warning("Filter by Office");
        return workPapers?.Where(x => x.ApprovalOpportunity.Regional.KantorPerwakilan == Filter.FilterOffice);
    }
}
