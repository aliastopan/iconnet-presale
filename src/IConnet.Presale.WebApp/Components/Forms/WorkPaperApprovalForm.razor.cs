using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkPaperApprovalForm : ComponentBase
{
    [Inject] public OptionService OptionService { get; set; } = default!;
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public EventCallback UnstageWorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeApprovalModel")]
    public WorkPaperApprovalModel? ApprovalModel { get; set; }

    protected bool IsLoading { get; set; } = false;
    protected bool IsCommitReady { get; set; } = false;

    private static readonly Icon _questionIcon = new Icons.Filled.Size20.QuestionCircle();
    private static readonly Icon _errorIcon = new Icons.Filled.Size20.ErrorCircle();
    private static readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

    protected Func<string, bool> OptionDisableOnProgress => option => option == OptionSelect.StatusApproval.OnProgress
        && ApprovalModel!.StatusApproval != OptionSelect.StatusApproval.OnProgress;

    protected bool DisableRootCause => ApprovalModel!.StatusApproval != OptionSelect.StatusApproval.Reject
        && ApprovalModel!.StatusApproval != OptionSelect.StatusApproval.ClosedLost;
    protected bool DisableOnProgress => ApprovalModel!.StatusApproval == OptionSelect.StatusApproval.OnProgress
        || (ApprovalModel!.StatusApproval == OptionSelect.StatusApproval.Approve && !ApprovalModel!.IsValidJarak());
    protected bool DisableForm => IsClosedLost()
        || ApprovalModel!.StatusApproval == OptionSelect.StatusApproval.Reject
        || ApprovalModel!.StatusApproval == OptionSelect.StatusApproval.ClosedLost;
    protected bool DisableCommit => !IsCommitReady;

    protected Icon LabelIconNamaPelanggan => GetValidationIcon(ApprovalModel!.HasilValidasi.ValidasiNama);
    protected Icon LabelIconNoTelepon => GetValidationIcon(ApprovalModel!.HasilValidasi.ValidasiNomorTelepon);
    protected Icon LabelIconEmail => GetValidationIcon(ApprovalModel!.HasilValidasi.ValidasiEmail);
    protected Icon LabelIconIdPln => GetValidationIcon(ApprovalModel!.HasilValidasi.ValidasiIdPln);
    protected Icon LabelIconAlamat => GetValidationIcon(ApprovalModel!.HasilValidasi.ValidasiAlamat);
    protected Icon ApprovalIcon => GetApprovalIcon(ApprovalModel!.StatusApproval);

    protected string CssBackgroundColorNamaPelanggan => GetCssBackgroundColorValueActual(ApprovalModel!.HasilValidasi.ValidasiNama);
    protected string CssBackgroundColorNomorTelepon => GetCssBackgroundColorValueActual(ApprovalModel!.HasilValidasi.ValidasiNomorTelepon);
    protected string CssBackgroundColorEmail => GetCssBackgroundColorValueActual(ApprovalModel!.HasilValidasi.ValidasiEmail);
    protected string CssBackgroundColorIdPln => GetCssBackgroundColorValueActual(ApprovalModel!.HasilValidasi.ValidasiIdPln);
    protected string CssBackgroundColorAlamat => GetCssBackgroundColorValueActual(ApprovalModel!.HasilValidasi.ValidasiAlamat);

    protected string CssBackgroundColorNamaPelangganPembetulan => GetCssBackgroundColorValueRevision(ApprovalModel!.HasilValidasi.ValidasiNama);
    protected string CssBackgroundColorNomorTeleponPembetulan => GetCssBackgroundColorValueRevision(ApprovalModel!.HasilValidasi.ValidasiNomorTelepon);
    protected string CssBackgroundColorEmailPembetulan => GetCssBackgroundColorValueRevision(ApprovalModel!.HasilValidasi.ValidasiEmail);
    protected string CssBackgroundColorIdPlnPembetulan => GetCssBackgroundColorValueRevision(ApprovalModel!.HasilValidasi.ValidasiIdPln);
    protected string CssBackgroundColorAlamatPembetulan => GetCssBackgroundColorValueRevision(ApprovalModel!.HasilValidasi.ValidasiAlamat);

    protected string CssStyleStrikethroughNamaPelanggan => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiNama);
    protected string CssStyleStrikethroughNomorTelepon => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiNomorTelepon);
    protected string CssStyleStrikethroughEmail => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiEmail);
    protected string CssStyleStrikethroughIdPln => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiIdPln);
    protected string CssStyleStrikethroughAlamat => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiAlamat);

    protected async Task OnCommitAsync()
    {
        IsLoading = true;

        var signatureApproval = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var approvalStatus = EnumProcessor.StringToEnum<ApprovalStatus>(ApprovalModel!.StatusApproval);
        var rootCause = approvalStatus == ApprovalStatus.Approve
            ? string.Empty
            : ApprovalModel!.RootCause;

        var prosesApproval = WorkPaper!.ProsesApproval.WithSignatureApproval(signatureApproval)
            .WithStatusApproval(approvalStatus)
            .WithVaTerbit(ApprovalModel!.NullableVaTerbit!.Value)
            .WithJarakShareLoc(ApprovalModel!.JarakShareLoc)
            .WithJarakICrmPlus(ApprovalModel!.JarakICrmPlus)
            .WithKeterangan(ApprovalModel!.Keterangan)
            .WithRootCause(rootCause);

        WorkPaper.ProsesApproval = prosesApproval;
        WorkPaper.WorkPaperLevel = WorkPaperLevel.DoneProcessing;

        var message = $"{signatureApproval.Alias} has commit chat/call approval to {ApprovalModel!.IdPermohonan}";
        await UpdateProsesApproval(WorkPaper, message);

        await UnstageWorkPaper.InvokeAsync();

        IsLoading = false;
    }

    protected async Task OnClipboardNamaPelangganAsync()
    {
        string namaPelanggan = ApprovalModel!.DataPelanggan.NamaPelanggan;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", namaPelanggan);
        ClipboardToast(namaPelanggan);
    }

    protected async Task OnClipboardPembetulanNamaPelangganAsync()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiNama != ValidationStatus.TidakSesuai)
        {
            return;
        }

        string pembetulanNamaPelanggan = ApprovalModel!.DataPembetulan.PembetulanNama;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", pembetulanNamaPelanggan);
        ClipboardToast(pembetulanNamaPelanggan);
    }

    protected async Task OnClipboardNomorTeleponAsync()
    {
        string nomorTelepon = ApprovalModel!.DataPelanggan.NomorTelepon;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", nomorTelepon);
        ClipboardToast(nomorTelepon);
    }

    protected async Task OnClipboardPembetulanNomorTeleponAsync()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiNomorTelepon != ValidationStatus.TidakSesuai)
        {
            return;
        }

        string pembetulanNomorTelepon = ApprovalModel!.DataPembetulan.PembetulanNomorTelepon;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", pembetulanNomorTelepon);
        ClipboardToast(pembetulanNomorTelepon);
    }

    protected async Task OnClipboardEmailAsync()
    {
        string email = ApprovalModel!.DataPelanggan.Email;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", email);
        ClipboardToast(email);
    }

    protected async Task OnClipboardPembetulanEmailAsync()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiEmail != ValidationStatus.TidakSesuai)
        {
            return;
        }

        string pembetulanEmail = ApprovalModel!.DataPembetulan.PembetulanEmail;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", pembetulanEmail);
        ClipboardToast(pembetulanEmail);
    }

    protected async Task OnClipboardIdPlnAsync()
    {
        string idPln = ApprovalModel!.DataPelanggan.IdPln;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", idPln);
        ClipboardToast(idPln);
    }

    protected async Task OnClipboardPembetulanIdPlnAsync()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiIdPln != ValidationStatus.TidakSesuai)
        {
            return;
        }

        string pembetulanIdPln = ApprovalModel!.DataPembetulan.PembetulanIdPln;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", pembetulanIdPln);
        ClipboardToast(pembetulanIdPln);
    }

    protected async Task OnClipboardAlamatAsync()
    {
        string alamat = ApprovalModel!.DataPelanggan.Alamat;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", alamat);
        ClipboardToast(alamat);
    }

    protected async Task OnClipboardPembetulanAlamatAsync()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiAlamat != ValidationStatus.TidakSesuai)
        {
            return;
        }

        string pembetulanAlamat = ApprovalModel!.DataPembetulan.PembetulanAlamat;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", pembetulanAlamat);
        ClipboardToast(pembetulanAlamat);
    }

    protected async Task OnClipboardShareLocAsync()
    {
        string latitudeLongitude = ApprovalModel!.DataCrmKoordinat.GetLatitudeLongitude();

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", latitudeLongitude);
        ClipboardToast(latitudeLongitude);

        LogSwitch.Debug("Copying {0}", latitudeLongitude);
    }

    protected async Task OnClipboardSplitterAsync()
    {
        string splitter = ApprovalModel!.Splitter;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", splitter);
        ClipboardToast(splitter);

        LogSwitch.Debug("Copying {0}", splitter);
    }

    protected void OnJarakShareLocChanged(int jarakShareLoc)
    {
        ApprovalModel!.JarakShareLoc = jarakShareLoc;
    }

    protected void OnJarakICrmChanged(int jarakICrmPlus)
    {
        ApprovalModel!.JarakICrmPlus = jarakICrmPlus;
    }

    protected void OnVaTerbit(DateTime? tanggalVaTerbit)
    {
        ApprovalModel!.NullableVaTerbit = tanggalVaTerbit;
    }

    protected void OnStatusApprovalChanged(string statusApproval)
    {
        ApprovalModel!.StatusApproval = statusApproval;

        if (ApprovalModel!.RootCause.IsNullOrWhiteSpace())
        {
            ApprovalModel!.RootCause = OptionService.RootCauseOptions.First();
        }
    }

    protected void OnRootCauseChanged(string rootCause)
    {
        ApprovalModel!.RootCause = rootCause;
    }

    protected void OnKeteranganChanged(string Keterangan)
    {
        ApprovalModel!.Keterangan = Keterangan;
    }

    protected bool IsClosedLost()
    {
        DateTime today = DateTimeService.DateTimeOffsetNow.Date;

        return WorkPaper!.ProsesValidasi.IsClosedLost(today);
    }

    private async Task UpdateProsesApproval(WorkPaper workPaper, string broadcastMessage)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);
        LogSwitch.Debug("Broadcast approval {message}", broadcastMessage);
    }

    private void ClipboardToast(string clipboard)
    {
        var intent = ToastIntent.Info;
        var message = $"Copy: {clipboard}";
        var timeout = 2500; // milliseconds

        ToastService.ShowToast(intent, message, timeout: timeout);
    }

    private static Icon GetValidationIcon(ValidationStatus section,
        string questionIconColor = "var(--info)",
        string errorIconColor = "var(--error)",
        string checkmarkIconColor = "var(--success)")
    {
        switch (section)
        {
            case ValidationStatus.TidakSesuai:
                return _errorIcon.WithColor(errorIconColor);
            case ValidationStatus.Sesuai:
                return _checkmarkIcon.WithColor(checkmarkIconColor);
            default:
                return _questionIcon.WithColor(questionIconColor);
        }
    }

    private static Icon GetApprovalIcon(string? approvalStatus)
    {
        return approvalStatus switch
        {
            string status when status == OptionSelect.StatusApproval.OnProgress => _questionIcon.WithColor("var(--info)"),
            string status when status == OptionSelect.StatusApproval.ClosedLost => _errorIcon.WithColor("var(--soft-black)"),
            string status when status == OptionSelect.StatusApproval.Reject => _errorIcon.WithColor("var(--error)"),
            string status when status == OptionSelect.StatusApproval.Approve => _checkmarkIcon.WithColor("var(--success)"),
            _ => throw new NotImplementedException(),
        };
    }

    private static string GetCssBackgroundColorValueActual(ValidationStatus section,
        string valid = "approval-value-bg-valid",
        string info = "approval-value-bg-info",
        string closedLost = "approval-value-bg-closed-lost")
    {
        switch (section)
        {
            case ValidationStatus.MenungguValidasi:
                return closedLost;
            case ValidationStatus.TidakSesuai:
                return info;
            case ValidationStatus.Sesuai:
                return valid;
            default:
                throw new NotImplementedException();
        }
    }

    private static string GetCssBackgroundColorValueRevision(ValidationStatus section,
        string info = "approval-value-bg-neutral",
        string revision = "approval-value-bg-revision")
    {
        switch (section)
        {
            case ValidationStatus.TidakSesuai:
                return revision;
            default:
                return info;
        }
    }

    private static string GetCssStyleStrikethrough(ValidationStatus validationStatus)
    {
        return validationStatus == ValidationStatus.TidakSesuai
            ? "text-decoration: line-through; color: #ababab !important; cursor: pointer !important;"
            : string.Empty;
    }
}
