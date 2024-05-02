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

    protected bool DisableSaveButton => StatusVerifikasi != OptionSelect.StatusVerifikasi.DataSesuai || JarakICrmPlusVerification <= 0;
    protected bool DisableRejectButton => StatusVerifikasi == OptionSelect.StatusVerifikasi.MenungguVerifikasi || StatusVerifikasi == OptionSelect.StatusVerifikasi.DataSesuai;

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
        if (IsDirectApproval && !DirectApproval.IsNullOrWhiteSpace())
        {
            await DirectApprovalAsync();
        }
        else
        {
            await VerifyCrmAsync();
        }

        await Dialog.CloseAsync(Content);
    }

    protected async Task OnWaitAsync()
    {
        PutOnWait();
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

    private void PutOnWait()
    {
        var prosesApproval = Content!.ProsesApproval
            .WithKeterangan(Keterangan)
            .WithJarakICrmPlus(JarakICrmPlusVerification);

        Content.OnWait = true;
        Content.ProsesApproval = prosesApproval;
        Content.LastModified = DateTimeService.DateTimeOffsetNow;
    }

    private async Task VerifyCrmAsync()
    {
        Content.OnWait = false;
        Content.Shift = SessionService.GetShift();
        Content.WorkPaperLevel = WorkPaperLevel.ImportVerified;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Verified;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var prosesApproval = Content!.ProsesApproval
            .WithKeterangan(Keterangan)
            .WithJarakICrmPlus(JarakICrmPlusVerification);

        Content.ProsesApproval = prosesApproval;
        Content.LastModified = DateTimeService.DateTimeOffsetNow;
    }

    private async Task DirectApprovalAsync()
    {
        var directApprovalSignature = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        Content.OnWait = false;
        Content.Shift = SessionService.GetShift();
        Content.WorkPaperLevel = WorkPaperLevel.ImportVerified;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Verified;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = directApprovalSignature;

        var prosesApproval = Content!.ProsesApproval
            .WithVaTerbit(DateTimeService.DateTimeOffsetNow.DateTime)
            .WithJarakICrmPlus(JarakICrmPlusVerification)
            .WithJarakShareLoc(JarakICrmPlusVerification)
            .WithStatusApproval(ApprovalStatus.Approve)
            .WithDirectApproval(DirectApproval)
            .WithKeterangan(Keterangan)
            .WithSignatureApproval(directApprovalSignature);

        Content.ProsesApproval = prosesApproval;
        Content.WorkPaperLevel = WorkPaperLevel.DoneProcessing;
        Content.LastModified = DateTimeService.DateTimeOffsetNow;
    }

    private async Task RejectCrmAsync()
    {
        if (StatusVerifikasi == OptionSelect.StatusVerifikasi.MenungguVerifikasi
            || StatusVerifikasi == OptionSelect.StatusVerifikasi.DataSesuai)
        {
            StatusVerifikasi = OptionSelect.StatusVerifikasi.DataTidakSesuai;
        }

        var rootCause = StatusVerifikasi;
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
            .WithKeterangan(Keterangan)
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
