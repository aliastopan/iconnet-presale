using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;

namespace IConnet.Presale.WebApp.Components.Pages;

public class ApprovalStagingPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private readonly string _pageName = "Approval staging page";
    private readonly static int _stagingLimit = 10;

    protected string GridTemplateCols => GetGridTemplateCols();
    protected FilterForm FilterComponent { get; set; } = default!;

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(ApprovalStagingPage());

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        PageName = _pageName;
        CacheFetchMode = CacheFetchMode.OnlyWaitingApproval;

        await base.OnInitializedAsync();

        ColumnWidth.SetColumnWidth(WorkPapers);
    }

    protected IQueryable<WorkPaper>? FilterWorkPapers()
    {
        if (FilterComponent is null)
        {
            return base.WorkPapers;
        }

        IQueryable<WorkPaper>? workPapers = FilterComponent.FilterWorkPapers(base.WorkPapers);

        ColumnWidth.SetColumnWidth(workPapers);
        return workPapers;
    }

    protected async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is null)
        {
            return;
        }

        var workPaper = row.Item;
        // Log.Warning("Selected row {0}", workPaper is null ? "null" : workPaper.ApprovalOpportunity.IdPermohonan);

        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = new TimeSpan(0, 5, 0);
        var timeRemaining = workPaper!.HelpdeskInCharge.GetDurationRemaining(now, duration);
        var label = timeRemaining > TimeSpan.Zero ? "Active" : "Expired";

        // Log.Warning("Time remaining: {0} {1}", timeRemaining, label);

        var isNotStaged = workPaper!.HelpdeskInCharge.IsEmptySignature();
        var hasStageExpired = workPaper!.HelpdeskInCharge.IsDurationExceeded(now, duration);
        var isOnGoingValidation = workPaper!.ProsesValidasi.IsOnGoing;

        await Task.CompletedTask;
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
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
            {ColumnWidth.LinkRekapChatHistoryPx}px
            {ColumnWidth.KeteranganValidasiPx}px
            {ColumnWidth.ContactWhatsAppPx}px
            {ColumnWidth.KantorPerwakilanPx}px";
    }

    private static TabNavigationModel ApprovalStagingPage()
    {
        return new TabNavigationModel("approval-staging", "Tampungan Approval", PageRoute.ApprovalStaging);
    }
}
