using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public class PresaleDataPageBase : WorkloadPageBase, IPageNavigation
{
    private bool _isInitialized = false;
    private bool _firstLoad = true;
    private IQueryable<WorkPaper>? _presaleData;
    private IQueryable<WorkPaper>? _filteredPresaleData;

    private DateTime _previousDateTimeMin;
    private DateTime _previousDateTimeMax;

    protected UserRole UserRole { get; private set; }
    protected bool EnableSelection { get; set; }

    protected string GridTemplateCols => GetGridTemplateCols();
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("presale-data", PageNavName.PresaleData, PageRoute.PresaleData);
    }

    protected override void OnInitialized()
    {
        PageName = PageNavName.PresaleData;
        PresaleDataFilter = PresaleDataFilter.OnlyDoneProcessing;

        TabNavigationManager.SelectTab(this);
    }

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            UserRole = await SessionService.GetUserRoleAsync();
            _presaleData = await WorkloadManager.GetWorkloadAsync(PresaleDataFilter);

            _previousDateTimeMin = SessionService.FilterPreference.TglPermohonanMin;
            _previousDateTimeMax = SessionService.FilterPreference.TglPermohonanMax;

            await LoadPresaleDataAsync();

            BroadcastService.Subscribe(OnUpdateWorkloadAsync);

            _isInitialized = true;
        }

        ColumnWidth.SetColumnWidth(WorkPapers);
    }

    protected async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (!EnableSelection || row.Item is null)
        {
            return;
        }

        await OpenReopenWorkloadDialogAsync(row.Item!);

        await Task.CompletedTask;
    }

    protected IQueryable<WorkPaper>? FilterWorkPapers()
    {
        if (FilterComponent is null)
        {
            return _presaleData;
        }

        if (FilterComponent.IsFiltered)
        {
            if (_filteredPresaleData is null || _firstLoad)
            {
                _firstLoad = false;

                return FilterComponent.FilterWorkPapers(_presaleData)?
                    .OrderByDescending(x => x.ApprovalOpportunity.TglPermohonan);
            }

            return _filteredPresaleData;
        }

        _filteredPresaleData = FilterComponent.FilterWorkPapers(_presaleData)?
            .OrderByDescending(x => x.ApprovalOpportunity.TglPermohonan);

        ColumnWidth.SetColumnWidth(_filteredPresaleData);

        FilterComponent.IsFiltered = true;

        return _filteredPresaleData;
    }

    protected async Task FilterRangeDateChangedAsync()
    {
        if (!IsCurrentRangeWithinPreviousRange())
        {
            _presaleData = null;
            await LoadPresaleDataAsync();

            _previousDateTimeMin = SessionService.FilterPreference.TglPermohonanMin;
            _previousDateTimeMax = SessionService.FilterPreference.TglPermohonanMax;

            this.StateHasChanged();
        }
    }

    protected override async Task OnUpdateWorkloadAsync(string message)
    {
        await LoadPresaleDataAsync();

        await InvokeAsync(() =>
        {
            ColumnWidth.SetColumnWidth(WorkPapers);
            StateHasChanged();
        });
    }

    protected async Task OpenReopenWorkloadDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Reopen Kertas Kerja (Reset)",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<ResetPresaleDataDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (WorkPaper)result.Data;

        await ResetPresaleDataAsync(dialogData);
    }

    private async Task ResetPresaleDataAsync(WorkPaper workPaper)
    {
        var resetSignature = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        workPaper = workPaper.ResetPresaleData(resetSignature);

        await WorkloadManager.ReinstateWorkloadAsync(workPaper);

        var broadcastMessage = $"Reinstating '{workPaper.ApprovalOpportunity.IdPermohonan}' presale negotiation";
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);
    }

    private async Task LoadPresaleDataAsync()
    {
        var dateTimeMin = SessionService.FilterPreference.TglPermohonanMin;
        var dateTimeMax = SessionService.FilterPreference.TglPermohonanMax;

        _presaleData = await WorkloadManager.GetArchivedPresaleDataAsync(dateTimeMin, dateTimeMax);
    }

    private bool IsCurrentRangeWithinPreviousRange()
    {
        var currentDateTimeMin = SessionService.FilterPreference.TglPermohonanMin.Date;
        var currentDateTimeMax = SessionService.FilterPreference.TglPermohonanMax.Date;

        if (currentDateTimeMin.Date < _previousDateTimeMin.Date)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.WorkPaperLevelPx}px
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.StatusApprovalPx}px
            {ColumnWidth.RootCausePx}px
            {ColumnWidth.NamaPemohonPx + 200}px
            {ColumnWidth.TelpPemohonPx + 180}px
            {ColumnWidth.EmailPemohonPx + 200}px
            {ColumnWidth.IdPlnPx + 180}px
            {ColumnWidth.AlamatPemohonPx + 200}px
            {ColumnWidth.ValidasiShareLocPx}px
            {ColumnWidth.NikPemohonPx}px
            {ColumnWidth.NpwpPemohonPx}px
            {ColumnWidth.KeteranganPx}px
            {ColumnWidth.TglChatCallMulaiPx}px
            {ColumnWidth.TglChatCallResponsPx}px
            {ColumnWidth.HelpdeskInChargePx}px
            {ColumnWidth.LinkChatHistoryPx}px
            {ColumnWidth.KeteranganValidasiPx}px
            {ColumnWidth.ContactWhatsAppPx}px
            {ColumnWidth.TglApprovalPx}px
            {ColumnWidth.PlanningAssetCoverageInChargePx}px
            {ColumnWidth.KeteranganApprovalPx}px
            {ColumnWidth.LayananPx}px
            {ColumnWidth.SumberPermohonanPx}px
            {ColumnWidth.StatusPermohonanPx}px
            {ColumnWidth.NamaAgenPx}px
            {ColumnWidth.EmailAgenPx}px
            {ColumnWidth.TelpAgenPx}px
            {ColumnWidth.MitraAgenPx}px
            {ColumnWidth.SplitterPx}px
            {ColumnWidth.SplitterGantiPx}px
            {ColumnWidth.RegionalPx}px
            {ColumnWidth.KantorPerwakilanPx}px
            {ColumnWidth.ProvinsiPx}px
            {ColumnWidth.KabupatenPx}px
            {ColumnWidth.KecamatanPx}px
            {ColumnWidth.KelurahanPx}px
            {ColumnWidth.LatitudePx}px
            {ColumnWidth.LongitudePx}px;";
    }
}
