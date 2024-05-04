using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;

namespace IConnet.Presale.WebApp.Components.Pages;

public class WorkloadPageBase : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public CommonDuplicateService CommonDuplicateService { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;

    private bool _isInitialized = false;
    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new PaginationState { ItemsPerPage = _itemPerPage };
    private readonly WorkloadColumnWidth _columnWidth = new WorkloadColumnWidth();

    private IQueryable<WorkPaper>? _workPapers;

    protected FilterForm FilterComponent { get; set; } = default!;
    protected string FilterSectionCss => SessionService.FilterPreference.ShowFilters ? "enable" : "filter-section-disable";
    protected string DisplayNoneStyle => WorkPapers is null ? "display: none;" : "";
    protected bool IsPageScrollDataGrid { get; set; } = false;
    protected bool EnablePageScrollToggle => _pagination.ItemsPerPage <= 10;
    protected string PaginationItemsPerPageOptions { get; set ;} = default!;
    protected PaginationState Pagination => _pagination;
    protected WorkloadColumnWidth ColumnWidth => _columnWidth;
    protected PresaleDataFilter PresaleDataFilter { get; set; } = PresaleDataFilter.OnlyImportVerified;
    protected virtual IQueryable<WorkPaper>? WorkPapers => _workPapers;

    protected GridSort<WorkPaper> SortByWorkPaperLevel => _workPapers.SortByWorkPaperLevel();
    protected GridSort<WorkPaper> SortByIdPermohonan => _workPapers.SortByIdPermohonan();
    protected GridSort<WorkPaper> SortByTglPermohonan => _workPapers.SortByTglPermohonan();
    protected GridSort<WorkPaper> SortByNamaPemohon => _workPapers.SortByNamaPemohon();
    protected GridSort<WorkPaper> SortByNomorTeleponPemohon => _workPapers.SortByNomorTeleponPemohon();
    protected GridSort<WorkPaper> SortByEmailPemohon => _workPapers.SortByEmailPemohon();
    protected GridSort<WorkPaper> SortByIdPln => _workPapers.SortByIdPln();
    protected GridSort<WorkPaper> SortByAlamatPemohon => _workPapers.SortByAlamatPemohon();
    protected GridSort<WorkPaper> SortByNikPemohon => _workPapers.SortByNikPemohon();
    protected GridSort<WorkPaper> SortByNpwpPemohon => _workPapers.SortByNpwpPemohon();
    protected GridSort<WorkPaper> SortByTglChatCallMulai => _workPapers.SortByTglChatCallMulai();
    protected GridSort<WorkPaper> SortByTglChatCallRespons => _workPapers.SortByTglChatCallRespons();
    protected GridSort<WorkPaper> SortByHelpdeskInCharge => _workPapers.SortByHelpdeskInCharge();
    protected GridSort<WorkPaper> SortByStatusApproval => _workPapers.SortByStatusApproval();
    protected GridSort<WorkPaper> SortByRootCause => _workPapers.SortByRootCause();
    protected GridSort<WorkPaper> SortByTglApproval => _workPapers.SortByTglApproval();
    protected GridSort<WorkPaper> SortByPlanningAssetCoverageInCharge => _workPapers.SortByPlanningAssetCoverageInCharge();
    protected GridSort<WorkPaper> SortByLayanan => _workPapers.SortByLayanan();
    protected GridSort<WorkPaper> SortBySumberPermohonan => _workPapers.SortBySumberPermohonan();
    protected GridSort<WorkPaper> SortByNamaAgen => _workPapers.SortByNamaAgen();
    protected GridSort<WorkPaper> SortByEmailAgen => _workPapers.SortByEmailAgen();
    protected GridSort<WorkPaper> SortByNomorTeleponAgen => _workPapers.SortByNomorTeleponAgen();
    protected GridSort<WorkPaper> SortByMitraAgen => _workPapers.SortByMitraAgen();
    protected GridSort<WorkPaper> SortBySplitter => _workPapers.SortBySplitter();
    protected GridSort<WorkPaper> SortByRegional => _workPapers.SortByRegional();
    protected GridSort<WorkPaper> SortByKantorPerwakilan => _workPapers.SortByKantorPerwakilan();
    protected GridSort<WorkPaper> SortByProvinsi => _workPapers.SortByProvinsi();
    protected GridSort<WorkPaper> SortByKabupaten => _workPapers.SortByKabupaten();
    protected GridSort<WorkPaper> SortByKecamatan => _workPapers.SortByKecamatan();
    protected GridSort<WorkPaper> SortByKelurahan => _workPapers.SortByKelurahan();

    protected GridSort<WorkPaper> SortByImportSignatureAlias => WorkPapers.SortByImportSignatureAlias();
    protected GridSort<WorkPaper> SortByImportVerificationSignatureAlias => WorkPapers.SortByImportVerificationSignatureAlias();

    protected string PageName { get; set; } = "Workload page (base)";
    protected virtual bool IsLoading { get; set; } = false;
    protected virtual bool IsRefreshPage { get; set; } = false;

    protected Icon ReinstatedIcon = new Icons.Filled.Size20.ArrowCounterclockwise().WithColor("var(--info-grey)");
    protected Icon OnWaitIcon = new Icons.Filled.Size20.Hourglass().WithColor("var(--pending-cyan)");
    protected Icon HasCommonDuplicateIcon = new Icons.Filled.Size20.ErrorCircle().WithColor("var(--error-red)");

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _workPapers = await WorkloadManager.GetWorkloadAsync(PresaleDataFilter);
            BroadcastService.Subscribe(OnUpdateWorkloadAsync);

            _isInitialized = true;
        }
    }

    protected virtual async Task RefreshPageAsync()
    {
        if(IsRefreshPage)
        {
            return;
        }

        // _workPapers = null!;

        IsRefreshPage = true;
        this.StateHasChanged();

        _workPapers = await WorkloadManager.GetWorkloadAsync(PresaleDataFilter);

        IsRefreshPage = false;
        StateHasChanged();
        this.StateHasChanged();
    }

    protected virtual async Task OnUpdateWorkloadAsync(string message)
    {
        _workPapers = await WorkloadManager.GetWorkloadAsync(PresaleDataFilter);

        // Log.Warning(message);

        // ensure component update is handle by UI thread
        await InvokeAsync(() =>
        {
            StateHasChanged();
            // Log.Warning($"Re-render '{PageName}'.");
        });
    }

    protected void OnItemsPerPageChanged(string ItemsPerPageString)
    {
        int itemsPerPage = int.Parse(ItemsPerPageString);
        _pagination.ItemsPerPage = itemsPerPage;
    }

    protected string GetWidthStyle(int widthPx, int offsetPx = 0)
    {
        return $"width: {widthPx + offsetPx}px;";
    }

    protected string GetPaginationStyle()
    {
        if (_pagination.ItemsPerPage <= 10 || !IsPageScrollDataGrid)
        {
            return "max-height: 364px !important";
        }

        return "min-height: 364px !important";
    }

    protected bool IsReinstated(WorkPaperLevel workPaperLevel)
    {
        return workPaperLevel == WorkPaperLevel.Reinstated;
    }

    protected bool HasCommonDuplicate(WorkPaper workPaper)
    {
        string idPln = workPaper.ApprovalOpportunity.Pemohon.IdPln;
        string email = workPaper.ApprovalOpportunity.Pemohon.Email;

        // var potentialDuplicate = CommonDuplicateService.PotentialDuplicates.FirstOrDefault(duplicate => (duplicate.IdPln == idPln || duplicate.Email == email)
        //     && duplicate.IdPermohonan != workPaper.ApprovalOpportunity.IdPermohonan);

        // if (potentialDuplicate is not null)
        // {
        //     Log.Warning("Duplicate {0} with {1}", workPaper.ApprovalOpportunity.IdPermohonan, duplicate.IdPermohonan);
        // }

        return CommonDuplicateService.PotentialDuplicates.Any(duplicate => (duplicate.IdPln == idPln || duplicate.Email == email)
            && duplicate.IdPermohonan != workPaper.ApprovalOpportunity.IdPermohonan);
    }
}
