namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class CrmVerificationDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected Func<string, bool> OptionDisable => option => option == OptionSelect.StatusVerifikasi.MenungguVerifikasi
        && StatusVerifikasi != OptionSelect.StatusVerifikasi.MenungguVerifikasi;

    protected bool DisableSaveButton => StatusVerifikasi != OptionSelect.StatusVerifikasi.DataSesuai || JarakICrmPlusVerification <= 0;
    protected bool DisableRejectButton => StatusVerifikasi != OptionSelect.StatusVerifikasi.DataTidakSesuai;

    public int JarakICrmPlusVerification { get; set; }
    public string Keterangan { get; set; } = string.Empty;
    public string StatusVerifikasi = OptionSelect.StatusVerifikasi.MenungguVerifikasi;

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

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
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
    }

    private async Task RejectCrmAsync()
    {
        Content.Shift = SessionService.GetShift();
        Content.WorkPaperLevel = WorkPaperLevel.ImportInvalid;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Invalid;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var rootCause = "DATA TIDAK VALID";

        var prosesApproval = Content.ProsesApproval
            .WithStatusApproval(ApprovalStatus.Rejected)
            .WithRootCause(rootCause);

        Content.ProsesApproval = prosesApproval;
    }

    private async Task ArchiveCrmAsync()
    {
        Content.WorkPaperLevel = WorkPaperLevel.ImportArchived;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Invalid;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };
    }

    protected async Task OnOpenGoogleMapAsync()
    {
        var url = Content.ApprovalOpportunity.Regional.Koordinat.GetGoogleMapLink();

        await JsRuntime.InvokeVoidAsync("open", url, "_blank");
    }
}
