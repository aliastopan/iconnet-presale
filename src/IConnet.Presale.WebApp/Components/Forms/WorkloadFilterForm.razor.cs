namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadFilterForm
{
    [Parameter] public EventCallback OnFilter { get; set; }

    private static string _filterOfficeDefault = EnumerableOptions.KantorPerwakilan.First();
    private string _filterOffice = _filterOfficeDefault;
    private string? _filterSearch;

    public bool IsFilterSet => FilterByOffice != _filterOfficeDefault;
    public string FilterByOffice => _filterOffice;
    public string? FilterSearch => _filterSearch;

    public async Task FilterByOfficeAsync(string filterOffice)
    {
        Log.Warning("Filter {0}", filterOffice);
        _filterOffice = filterOffice;
        _filterSearch = string.Empty;

        await OnFilter.InvokeAsync();
    }

    public async Task FilterBySearchAsync(string filterSearch)
    {
        Log.Warning("Filter Search {0}", filterSearch);
        _filterSearch = filterSearch;
        _filterOffice = _filterOfficeDefault;

        await OnFilter.InvokeAsync();
    }

    public IQueryable<WorkPaper>? FilterWorkPapers(IQueryable<WorkPaper>? workPapers)
    {
        if (!IsFilterSet)
        {
            if (FilterSearch.HasValue())
            {
                Log.Warning("Filter by Search");
                return workPapers?.Where(x => x.ApprovalOpportunity.IdPermohonan == FilterSearch);
            }
            else
            {
                return workPapers;
            }
        }

        Log.Warning("Filter by Office");
        return workPapers?.Where(x => x.ApprovalOpportunity.Regional.KantorPerwakilan == FilterByOffice);
    }
}
