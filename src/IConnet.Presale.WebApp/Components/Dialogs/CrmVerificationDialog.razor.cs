namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class CrmVerificationDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public OptionService OptionService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    private bool _isInitialized;

    protected Func<string, bool> OptionDisable => option => option == OptionSelect.StatusVerifikasi.MenungguVerifikasi
        && StatusVerifikasi != OptionSelect.StatusVerifikasi.MenungguVerifikasi;

    protected bool DisableSaveButton => StatusVerifikasi != OptionSelect.StatusVerifikasi.DataSesuai || JarakICrmPlusVerification <= 0;
    protected bool DisableRejectButton => StatusVerifikasi != OptionSelect.StatusVerifikasi.DataTidakSesuai;

    public int JarakICrmPlusVerification { get; set; }
    public string Keterangan { get; set; } = string.Empty;
    public string StatusVerifikasi = OptionSelect.StatusVerifikasi.MenungguVerifikasi;
    public string DirectApproval { get; set; } = string.Empty;
    public bool IsDirectApproval { get; set; }

    protected void OnJarakICrmChanged(int jarakShareLoc)
    {
        JarakICrmPlusVerification = jarakShareLoc;
    }

    protected void OnKeteranganChanged(string keterangan)
    {
        Keterangan = keterangan;
    }

    protected void OnStatusVerifikasiChanged(string statusVerifikasi)
    {
        StatusVerifikasi = statusVerifikasi;
    }

    protected void OnIsDirectApprovalChanged(bool isDirectApproval)
    {
        if (!_isInitialized)
        {
            DirectApproval = OptionService.DirectApprovalOptions.First();
        }

        _isInitialized = true;
        IsDirectApproval = isDirectApproval;
    }

    protected void OnDirectApprovalChanged(string directApproval)
    {
        DirectApproval = directApproval;
    }

    protected async Task SaveAsync()
    {
        await VerifyCrmAsync();
        await Dialog.CloseAsync(Content);
    }

    protected async Task RejectAsync()
    {
        await RejectCrmAsync();
        await Dialog.CloseAsync(Content);
    }

    protected async Task ArchiveAsync()
    {
        await ArchiveCrmAsync();
        await Dialog.CloseAsync(Content);
    }

    protected async Task DeleteAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = Content.ApprovalOpportunity.IdPermohonan,
            TrapFocus = true,
            PreventDismissOnOverlayClick = true,
            Width = "650px",
        };

        var dialog = await DialogService.ShowDialogAsync<CrmDeleteConfirmationDialog>(parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            DeleteCrm();
            await Dialog.CloseAsync(Content);
        }
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync(Content);
    }

    private async Task VerifyCrmAsync()
    {
        Content.Shift = SessionService.GetShift();
        Content.WorkPaperLevel = WorkPaperLevel.ImportVerified;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Verified;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var prosesApproval = Content!.ProsesApproval.WithJarakICrmPlus(JarakICrmPlusVerification);

        Content.ProsesApproval = prosesApproval;
        Content.LastModified = DateTimeService.DateTimeOffsetNow;
    }

    private async Task RejectCrmAsync()
    {
        var rootCause = "DATA TIDAK VALID";
        var rejectSignature = ActionSignature.Empty();

        Content.Shift = SessionService.GetShift();
        Content.WorkPaperLevel = WorkPaperLevel.ImportInvalid;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Invalid;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        Content.SetHelpdeskInCharge(rejectSignature);
        Content.SetPlanningAssetCoverageInCharge(rejectSignature);

        var prosesValidasi = Content.ProsesValidasi
            .WithSignatureChatCallMulai(rejectSignature)
            .WithSignatureChatCallRespons(rejectSignature);

        var prosesApproval = Content.ProsesApproval
            .WithStatusApproval(ApprovalStatus.Reject)
            .WithRootCause(rootCause)
            .WithSignatureApproval(rejectSignature);

        Content.ProsesValidasi = prosesValidasi;
        Content.ProsesApproval = prosesApproval;
        Content.LastModified = DateTimeService.DateTimeOffsetNow;
    }

    private async Task ArchiveCrmAsync()
    {
        Content.WorkPaperLevel = WorkPaperLevel.Reinstated;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Invalid;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };
        Content.LastModified = DateTimeService.DateTimeOffsetNow;
    }

    private void DeleteCrm()
    {
        Content.WorkPaperLevel = WorkPaperLevel.ImportInvalid;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.ToBeDeleted;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = ActionSignature.Empty();
    }

    protected async Task OnOpenGoogleMapAsync()
    {
        var url = Content.ApprovalOpportunity.Regional.Koordinat.GetGoogleMapLink();

        await JsRuntime.InvokeVoidAsync("open", url, "_blank");
    }

    protected string GetDirectApprovalToggleCss()
    {
        return IsDirectApproval
            ? "direct-approval-enable"
            : "direct-approval-disable";
    }
}
