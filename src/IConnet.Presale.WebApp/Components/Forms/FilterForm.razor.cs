namespace IConnet.Presale.WebApp.Components.Forms;

public partial class FilterForm : ComponentBase
{
    [Inject] public OptionService OptionService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;

    [Parameter] public EventCallback OnFilter { get; set; }
    [Parameter] public EventCallback OnDateRangeFilterChanged { get; set; }
    [Parameter] public bool IncludeApprovalStatus { get; set; }

    private FilterModel _filterModel = default!;

    public bool IsFiltered { get; set; }

    public FilterModel FilterModel => _filterModel;
    public string FilterDateTimeRangeLabel => GetDaysRangeLabel();

    protected override void OnInitialized()
    {
        var today = DateTimeService.DateTimeOffsetNow.DateTime;
        var filterPreference = SessionService.FilterPreference;
        var kantorPerwakilanOptions = OptionService.KantorPerwakilanOptions;

        IsFiltered = false;

        _filterModel = new FilterModel(today, filterPreference, kantorPerwakilanOptions);
    }

    protected override async Task OnInitializedAsync()
    {
        FilterModel.FilterOffice = SessionService.FilterPreference.KantorPerwakilan;
        FilterModel.NullableFilterDateTimeMin = SessionService.FilterPreference.TglPermohonanMin;
        FilterModel.NullableFilterDateTimeMax = SessionService.FilterPreference.TglPermohonanMax;

        IsFiltered = false;

        await OnFilter.InvokeAsync();
    }

    protected async Task FilterByStatusApprovalAsync(string filterStatusApproval)
    {
        FilterModel.FilterSearch = string.Empty;
        FilterModel.FilterStatusApproval = filterStatusApproval;

        IsFiltered = false;

        if (OnFilter.HasDelegate)
        {
            await OnFilter.InvokeAsync();
        }
    }

    protected async Task FilterByOfficeAsync(string filterOffice)
    {
        FilterModel.FilterSearch = string.Empty;
        FilterModel.FilterOffice = filterOffice;
        SessionService.FilterPreference.KantorPerwakilan = filterOffice;

        IsFiltered = false;

        if (OnFilter.HasDelegate)
        {
            await OnFilter.InvokeAsync();
        }
    }

    protected async Task FilterBySearchAsync(string filterSearch)
    {
        FilterModel.ResetColumnFilters();
        FilterModel.FilterOffice = FilterModel.FilterOfficeDefault;
        SessionService.FilterPreference.KantorPerwakilan = FilterModel.FilterOffice;
        FilterModel.FilterSearch = filterSearch;

        IsFiltered = false;

        if (OnFilter.HasDelegate)
        {
            await OnFilter.InvokeAsync();
        }
    }

    protected async Task SetFilterDateTimeStartAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        var dateTime = nullableDateTime.Value;
        if (dateTime > FilterModel.NullableFilterDateTimeMax)
        {
            dateTime = FilterModel.FilterDateTimeMax.AddDays(-1);
        }

        FilterModel.NullableFilterDateTimeMin = dateTime;
        SessionService.FilterPreference.TglPermohonanMin = dateTime;

        IsFiltered = false;

        if (OnFilter.HasDelegate)
        {
            await OnFilter.InvokeAsync();
        }

        if (OnDateRangeFilterChanged.HasDelegate)
        {
            await OnDateRangeFilterChanged.InvokeAsync();
        }
    }

    protected async Task SetFilterDateTimeEndAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        var dateTime = nullableDateTime.Value;
        if (dateTime < FilterModel.NullableFilterDateTimeMin)
        {
            dateTime = FilterModel.FilterDateTimeMin.AddDays(1);
        }

        FilterModel.NullableFilterDateTimeMax = dateTime;
        SessionService.FilterPreference.TglPermohonanMax = dateTime;

        IsFiltered = false;

        if (OnFilter.HasDelegate)
        {
            await OnFilter.InvokeAsync();
        }

        if (OnDateRangeFilterChanged.HasDelegate)
        {
            await OnDateRangeFilterChanged.InvokeAsync();
        }
    }

    protected async Task ResetFilterAsync()
    {
        var today = DateTimeService.DateTimeOffsetNow.DateTime;
        FilterModel.ResetFilters(today, SessionService.FilterPreference);

        IsFiltered = false;

        await OnFilter.InvokeAsync();
    }

    public IQueryable<WorkPaper>? BaseFilter(IQueryable<WorkPaper>? workPapers)
    {
        // prioritize filter search
        if (FilterModel.FilterSearch.HasValue())
        {
            return workPapers?.Where(x => x.ApprovalOpportunity.IdPermohonan == FilterModel.FilterSearch);
        }
        else
        {
            if (FilterModel.IsFilterOfficeSpecified)
            {
                workPapers = workPapers?.Where(x => x.ApprovalOpportunity.Regional.KantorPerwakilan == FilterModel.FilterOffice);
            }

            if (IncludeApprovalStatus && !FilterModel.FilterStatusApproval.IsNullOrWhiteSpace())
            {
                var approvalStatus = EnumProcessor.StringToEnum<ApprovalStatus>(FilterModel.FilterStatusApproval);

                workPapers = workPapers?.Where(x => x.ProsesApproval.StatusApproval == approvalStatus);
            }

            return workPapers?.Where(x => x.ApprovalOpportunity.TglPermohonan.Date >= FilterModel.FilterDateTimeMin.Date
                && x.ApprovalOpportunity.TglPermohonan.Date <= FilterModel.FilterDateTimeMax.Date);
        }
    }

    public IQueryable<WorkPaper>? FilterWorkPapers(IQueryable<WorkPaper>? workPapers)
    {
        return BaseFilter(workPapers)?
            .Where(x =>
                (!FilterModel.IdPermohonan.HasValue() || x.ApprovalOpportunity.IdPermohonan
                    .Contains(FilterModel.IdPermohonan, StringComparison.CurrentCultureIgnoreCase)) &&
                (!FilterModel.NamaPemohon.HasValue() || x.ApprovalOpportunity.Pemohon.NamaPelanggan
                    .Contains(FilterModel.NamaPemohon, StringComparison.CurrentCultureIgnoreCase)) &&
                (!FilterModel.IdPln.HasValue() || x.ApprovalOpportunity.Pemohon.IdPln
                    .Contains(FilterModel.IdPln, StringComparison.CurrentCultureIgnoreCase)) &&
                (!FilterModel.NomorTeleponPemohon.HasValue() || x.ApprovalOpportunity.Pemohon.NomorTelepon
                    .Contains(FilterModel.NomorTeleponPemohon, StringComparison.CurrentCultureIgnoreCase)) &&
                (!FilterModel.EmailPemohon.HasValue() || x.ApprovalOpportunity.Pemohon.Email
                    .Contains(FilterModel.EmailPemohon, StringComparison.CurrentCultureIgnoreCase)) &&
                (!FilterModel.AlamatPemohon.HasValue() || x.ApprovalOpportunity.Pemohon.Alamat
                    .Contains(FilterModel.AlamatPemohon, StringComparison.CurrentCultureIgnoreCase)) &&
                (!FilterModel.Splitter.HasValue() || x.ApprovalOpportunity.Splitter
                    .Contains(FilterModel.Splitter, StringComparison.CurrentCultureIgnoreCase))
            );
    }

    private string GetDaysRangeLabel()
    {
        var currentDate = DateTimeService.DateTimeOffsetNow.Date;
        var isToday = FilterModel.FilterDateTimeMax.Date == currentDate;

        return isToday
            ? $"{FilterModel.FilterDateTimeDifference.Days} Hari Terakhir"
            : $"Rentang {FilterModel.FilterDateTimeDifference.Days} Hari";
    }
}
