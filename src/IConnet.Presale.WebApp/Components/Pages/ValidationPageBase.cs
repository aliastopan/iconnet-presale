using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Pages;

public class ValidationPageBase : WorkloadPageBase, IPageNavigation
{
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

    private bool _firstLoad = true;
    private IQueryable<WorkPaper>? _filteredWorkPapers;

    private Guid _sessionId;
    private readonly List<WorkPaperValidationModel> _validationModels = new List<WorkPaperValidationModel>();
    private readonly GridSort<WorkPaper> _sortByStagingStatus = GridSort<WorkPaper>
        .ByAscending(workPaper => workPaper.SignatureHelpdeskInCharge.TglAksi);

    protected string GridTemplateCols => GetGridTemplateCols();
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    protected GridSort<WorkPaper> SortByStagingStatus => _sortByStagingStatus;
    protected WorkPaper? ActiveWorkPaper { get; set; }
    protected WorkPaperValidationModel? ActiveValidationModel { get; set; }

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("validation", PageNavName.Validation, PageRoute.Validation);
    }

    protected override void OnInitialized()
    {
        PageName = PageNavName.Validation;
        PresaleDataFilter = PresaleDataFilter.OnlyValidating;

        TabNavigationManager.SelectTab(this);

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        _sessionId = await SessionService.GetUserAccountIdAsync();

        await base.OnInitializedAsync();

        if (WorkPapers is null)
        {
            return;
        }

        foreach (var workPaper in WorkPapers)
        {
            _validationModels.Add(new WorkPaperValidationModel(workPaper));
        }
    }

    protected IQueryable<WorkPaper>? FilterWorkPapers()
    {
        if (FilterComponent is null)
        {
            return base.WorkPapers;
        }

        if (FilterComponent.IsFiltered)
        {
            if (_filteredWorkPapers is null || _firstLoad)
            {
                _firstLoad = false;

                return FilterComponent.FilterWorkPapers(base.WorkPapers)?
                    .Where(x => x.SignatureHelpdeskInCharge.AccountIdSignature == _sessionId
                        && x.ProsesValidasi.IsOnGoing())
                    .OrderByDescending(x => x.ApprovalOpportunity.TglPermohonan);
            }

            return _filteredWorkPapers;
        }

        _filteredWorkPapers = FilterComponent.FilterWorkPapers(base.WorkPapers)?
            .Where(x => x.SignatureHelpdeskInCharge.AccountIdSignature == _sessionId
                && x.ProsesValidasi.IsOnGoing())
            .OrderByDescending(x => x.ApprovalOpportunity.TglPermohonan);

        ColumnWidth.SetColumnWidth(_filteredWorkPapers);

        FilterComponent.IsFiltered = true;

        return _filteredWorkPapers;
    }

    protected async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is null)
        {
            return;
        }

        if (!IsStillInCharge(row.Item))
        {
            await OpenRestagingDialogAsync(row.Item);
            return;
        }

        ActiveWorkPaper = row.Item;
        ActiveValidationModel = _validationModels
            .FirstOrDefault(x => x.IdPermohonan == row.Item.ApprovalOpportunity.IdPermohonan);

        ActiveValidationModel!.NullableTanggalRespons = DateTimeService.DateTimeOffsetNow.DateTime;
        ActiveValidationModel!.NullableWaktuRespons = DateTimeService.DateTimeOffsetNow.DateTime;
    }

    public void DeselectWorkPaper()
    {
        ActiveWorkPaper = null;
        ActiveValidationModel = null;
    }

    protected async Task OpenRestagingDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Tampung Kertas Kerja",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<ValidationStagingAlertDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (WorkPaper)result.Data;
        if (dialogData.SignatureHelpdeskInCharge.IsEmptySignature())
        {
            await UnstageWorkPaperAsync(dialogData);

            return;
        }

        await RestageWorkPaperAsync(dialogData);
    }

    protected async Task ScrollToValidationForm()
    {
        var elementId = "validation-id";
        await JsRuntime.InvokeVoidAsync("scrollToElement", elementId);
    }

    protected bool IsStillInCharge(WorkPaper workPaper)
    {
        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = InChargeDuration.ValidationDuration;

        return !workPaper.SignatureHelpdeskInCharge.IsDurationExceeded(now, duration);
    }

    private async Task RestageWorkPaperAsync(WorkPaper workPaper)
    {
        ActiveWorkPaper = workPaper;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var broadcastMessage = $"Workload '{workPaper.ApprovalOpportunity.IdPermohonan}' staging claim has been extended";
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);
    }

    private async Task UnstageWorkPaperAsync(WorkPaper workPaper)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var broadcastMessage = $"Workload '{workPaper.ApprovalOpportunity.IdPermohonan}' is no longer staged";
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.StagingStatusPx}px
            {ColumnWidth.InChargeImportPx}px
            {ColumnWidth.InChargeVerificationPx}px
            {ColumnWidth.HelpdeskInChargePx}px
            {ColumnWidth.ShiftPx}px
            {ColumnWidth.TglChatCallMulaiPx}px
            {ColumnWidth.ValidasiNamaPelangganPx}px
            {ColumnWidth.ValidasiNomorTelpPx}px
            {ColumnWidth.ValidasiEmailPx}px
            {ColumnWidth.ValidasiIdPlnPx}px
            {ColumnWidth.ValidasiAlamatPx}px
            {ColumnWidth.ValidasiShareLocPx}px
            {ColumnWidth.TglChatCallResponsPx}px
            {ColumnWidth.LinkChatHistoryPx}px
            {ColumnWidth.KeteranganValidasiPx}px
            {ColumnWidth.ContactWhatsAppPx}px
            {ColumnWidth.KantorPerwakilanPx}px";
    }
}
