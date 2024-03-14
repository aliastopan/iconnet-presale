using System.Text.RegularExpressions;
using IConnet.Presale.Shared.Validations;
using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkPaperValidationForm : ComponentBase
{
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

    [CascadingParameter(Name = "CascadeValidationModel")]
    public WorkPaperValidationModel? ValidationModel { get; set; }

    // private readonly Icon _questionIcon = new Icons.Filled.Size20.QuestionCircle();
    // private readonly Icon _errorIcon = new Icons.Filled.Size20.ErrorCircle();
    // private readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

    protected bool IsLoading { get; set; } = false;
    protected bool IsCommitReady { get; set; } = false;

    protected bool DisableOnBeforeContact => !(ValidationModel?.IsChatCallMulai ?? false);

    protected Func<string, bool> OptionDisableNamaPelanggan => option => option == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiNama != OptionSelect.StatusValidasi.MenungguValidasi;
    protected Func<string, bool> OptionDisableNoTelepon => option => option == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiNomorTelepon != OptionSelect.StatusValidasi.MenungguValidasi;
    protected Func<string, bool> OptionDisableEmail => option => option == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiEmail != OptionSelect.StatusValidasi.MenungguValidasi;
    protected Func<string, bool> OptionDisableIdPln => x => x == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiIdPln != OptionSelect.StatusValidasi.MenungguValidasi;
    protected Func<string, bool> OptionDisableAlamat => x => x == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiAlamat != OptionSelect.StatusValidasi.MenungguValidasi;

    protected Icon LabelIconNamaPelanggan => GetValidationIcon(ValidationModel?.ValidasiNama);
    protected Icon LabelIconNoTelepon => GetValidationIcon(ValidationModel?.ValidasiNomorTelepon);
    protected Icon LabelIconEmail => GetValidationIcon(ValidationModel?.ValidasiEmail);
    protected Icon LabelIconIdPln => GetValidationIcon(ValidationModel?.ValidasiIdPln);
    protected Icon LabelIconAlamat => GetValidationIcon(ValidationModel?.ValidasiAlamat);
    protected Icon LabelIconCrmKoordinat => GetValidationIcon(ValidationModel?.ValidasiCrmKoordinat, errorIconColor: "var(--warning)");

    protected string CssBackgroundColorNamaPelanggan => GetCssBackgroundColor(ValidationModel?.ValidasiNama);
    protected string CssBackgroundColorNomorTelepon => GetCssBackgroundColor(ValidationModel?.ValidasiNomorTelepon);
    protected string CssBackgroundColorEmail => GetCssBackgroundColor(ValidationModel?.ValidasiEmail);
    protected string CssBackgroundColorIdPln => GetCssBackgroundColor(ValidationModel?.ValidasiIdPln);
    protected string CssBackgroundColorAlamat => GetCssBackgroundColor(ValidationModel?.ValidasiAlamat);
    protected string CssBackgroundColorCrmKoordinat => GetCssBackgroundColor(ValidationModel?.ValidasiCrmKoordinat, invalid: "validation-value-bg-warning");

    protected bool DisableTextFieldNamaPelanggan => ValidationModel?.ValidasiNama != OptionSelect.StatusValidasi.TidakSesuai;
    protected bool DisableTextFieldNoTelepon => ValidationModel?.ValidasiNomorTelepon != OptionSelect.StatusValidasi.TidakSesuai;
    protected bool DisableTextFieldEmail => ValidationModel?.ValidasiEmail != OptionSelect.StatusValidasi.TidakSesuai;
    protected bool DisableTextFieldIdPln => ValidationModel?.ValidasiIdPln != OptionSelect.StatusValidasi.TidakSesuai;
    protected bool DisableTextAreaAlamatPelanggan => ValidationModel?.ValidasiAlamat != OptionSelect.StatusValidasi.TidakSesuai;

    protected bool DisableCommitToggle => !EnableCommitButton();
    protected bool DisableCommit => !IsCommitReady;

    protected async Task OnCommitAsync()
    {
        IsLoading = true;

        var coordinateShareLoc = new Coordinate(ValidationModel!.ShareLoc);
        var signatureChatCallRespons = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var parameterValidasi = WorkPaper!.ProsesValidasi.ParameterValidasi.WithShareLoc(coordinateShareLoc);
        var prosesValidasi = WorkPaper.ProsesValidasi
            .WithParameterValidasi(parameterValidasi)
            .WithSignatureChatCallRespons(signatureChatCallRespons)
            .WithWaktuTanggalRespons(ValidationModel.GetWaktuTanggalRespons())
            .WithLinkChatHistory(ValidationModel.LinkChatHistory)
            .WithKeterangan(ValidationModel.Keterangan);

        WorkPaper.ProsesValidasi = prosesValidasi;
        WorkPaper.WorkPaperLevel = WorkPaperLevel.WaitingApproval;
        WorkPaper.Shift = SessionService.GetShift();

        var message = $"{signatureChatCallRespons.Alias} has commit chat/call validation to {WorkPaper.ApprovalOpportunity.IdPermohonan}";
        await UpdateProsesValidasi(WorkPaper, message);

        await UnstageWorkPaper.InvokeAsync();

        IsLoading = false;
    }

    protected async Task OnClosedLostAsync()
    {
        var prosesValidasi = WorkPaper!.ProsesValidasi.WithKeterangan(ValidationModel!.Keterangan);

        WorkPaper!.ProsesValidasi = prosesValidasi;
        WorkPaper!.WorkPaperLevel = WorkPaperLevel.WaitingApproval;

        var message = $"{WorkPaper.ApprovalOpportunity.IdPermohonan} is CLOSED LOST";

        Task[] tasks =
        [
            UpdateProsesValidasi(WorkPaper, message),
            UnstageWorkPaper.InvokeAsync()
        ];

        await Task.WhenAll(tasks);
    }

    protected bool IsClosedLost()
    {
        DateTime today = DateTimeService.DateTimeOffsetNow.Date;

        return WorkPaper!.ProsesValidasi.IsClosedLost(today);
    }

    protected string GetAgingChatCallMulaiString()
    {
        DateTime today = DateTimeService.DateTimeOffsetNow.Date;
        TimeSpan agingChatCallMulai = WorkPaper!.ProsesValidasi.GetAgingChatCallMulai(today);

        if (agingChatCallMulai.TotalHours < 1)
        {
            return "kurang dari 1 Jam";
        }
        else
        {
            int totalDays = (int)agingChatCallMulai.TotalDays;
            int hours = agingChatCallMulai.Hours;

            return $"{totalDays} Hari {hours} Jam";
        }
    }

    protected async Task OnClipboardNamaPelangganAsync()
    {
        string namaPelanggan = ValidationModel!.DataPelanggan.NamaPelanggan;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", namaPelanggan);
        ClipboardToast(namaPelanggan);
    }

    protected async Task OnClipboardNomorTeleponAsync()
    {
        string nomorTelepon = ValidationModel!.DataPelanggan.NomorTelepon;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", nomorTelepon);
        ClipboardToast(nomorTelepon);
    }

    protected async Task OnClipboardEmailAsync()
    {
        string email = ValidationModel!.DataPelanggan.Email;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", email);
        ClipboardToast(email);
    }

    protected async Task OnClipboardIdPlnAsync()
    {
        string idPln = ValidationModel!.DataPelanggan.IdPln;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", idPln);
        ClipboardToast(idPln);
    }

    protected async Task OnClipboardAlamatAsync()
    {
        string alamat = ValidationModel!.DataPelanggan.Alamat;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", alamat);
        ClipboardToast(alamat);
    }

    protected async Task OnClipboardShareLocAsync()
    {
        string latitudeLongitude = ValidationModel!.DataCrmKoordinat.GetLatitudeLongitude();

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", latitudeLongitude);
        ClipboardToast(latitudeLongitude);
    }

    protected void OpenDialog()
    {
        LogSwitch.Debug("Chat Template Dialog");
    }

    protected async Task OpenChatTemplateDialogAsync()
    {
        LogSwitch.Debug("Chat Template Dialog");

        var parameters = new DialogParameters()
        {
            Title = "Chat Template",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<ChatTemplateDialog>(WorkPaper!, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }
    }

    protected async Task UpdateChatCallMulaiSignature()
    {
        var signatureChatCallMulai = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var prosesValidasi = WorkPaper!.ProsesValidasi.WithSignatureChatCallMulai(signatureChatCallMulai);

        WorkPaper.ProsesValidasi = prosesValidasi;

        var message = $"{signatureChatCallMulai.Alias} has began chat/call to {WorkPaper.ApprovalOpportunity.IdPermohonan}";
        await UpdateProsesValidasi(WorkPaper, message);

        ValidationModel!.MulaiChatCall();
    }

    protected async Task ValidasiProperty(string propertyName, string statusValidasi)
    {
        var validationStatus = EnumProcessor.StringToEnum<ValidationStatus>(statusValidasi);
        var parameterValidasi = WorkPaper!.ProsesValidasi.ParameterValidasi.Validasi(propertyName, validationStatus);
        var prosesValidasi = WorkPaper.ProsesValidasi.WithParameterValidasi(parameterValidasi);

        WorkPaper.ProsesValidasi = prosesValidasi;

        var broadcastMessage = $"Validating: [{propertyName}:{WorkPaper.ProsesValidasi.ParameterValidasi.GetValidationStatus(propertyName)}]";
        await UpdateProsesValidasi(WorkPaper, broadcastMessage);
    }

    protected async Task PembetulanProperty(string propertyName, string pembetulan)
    {
        var pembetulanValidasi = WorkPaper!.ProsesValidasi.PembetulanValidasi.Pembetulan(propertyName, pembetulan);
        var prosesValidasi = WorkPaper.ProsesValidasi.WithPembetulanValidasi(pembetulanValidasi);

        WorkPaper.ProsesValidasi = prosesValidasi;

        var broadcastMessage = $"Correction: [{propertyName}:{WorkPaper.ProsesValidasi.PembetulanValidasi.GetPembetulan(propertyName)}]";
        await UpdateProsesValidasi(WorkPaper, broadcastMessage);
    }

    protected async Task OnValidateNamaPelangganChangedAsync(string statusValidasi)
    {
        string namaPelanggan = ValidationParameterPropertyNames.ValidasiNama;
        ValidationModel!.ValidasiNama = statusValidasi;

        await ValidasiProperty(namaPelanggan, statusValidasi);
    }

    protected async Task OnValidateNomorTeleponChangedAsync(string statusValidasi)
    {
        string nomorTelepon = ValidationParameterPropertyNames.ValidasiNomorTelepon;
        ValidationModel!.ValidasiNomorTelepon = statusValidasi;

        await ValidasiProperty(nomorTelepon, statusValidasi);
    }

    protected async Task OnValidateEmailChangedAsync(string statusValidasi)
    {
        string email = ValidationParameterPropertyNames.ValidasiEmail;
        ValidationModel!.ValidasiEmail = statusValidasi;

        await ValidasiProperty(email, statusValidasi);
    }

    protected async Task OnValidateIdPlnChangedAsync(string statusValidasi)
    {
        string idPln = ValidationParameterPropertyNames.ValidasiIdPln;
        ValidationModel!.ValidasiIdPln = statusValidasi;

        await ValidasiProperty(idPln, statusValidasi);
    }

    protected async Task OnValidateAlamatChangedAsync(string statusValidasi)
    {
        string alamat = ValidationParameterPropertyNames.ValidasiAlamat;
        ValidationModel!.ValidasiAlamat = statusValidasi;

        await ValidasiProperty(alamat, statusValidasi);
    }

    protected void OnShareLocChanged(string shareLoc)
    {
        if (!LatitudeLongitude().IsMatch(shareLoc))
        {
            ValidationModel!.ShareLoc = ValidationModel.ShareLoc;
            // LogSwitch.Debug("Invalid share loc format: {0}", shareLoc);

            return;
        }

        ValidationModel!.ShareLoc = shareLoc;
        ValidationModel.ValidasiCrmKoordinat = ValidationModel!.ShareLoc.Equals(WorkPaper!.ApprovalOpportunity.Regional.Koordinat.GetLatitudeLongitude())
            ? OptionSelect.StatusValidasi.Sesuai
            : OptionSelect.StatusValidasi.TidakSesuai;

        // LogSwitch.Debug("Share loc: {0}", shareLoc);
    }

    protected void OnWaktuResponsChanged(DateTime? waktuRespons)
    {
        ValidationModel!.NullableWaktuRespons = waktuRespons;
        // LogSwitch.Debug("Waktu respons: {0}", waktuRespons!.Value.TimeOfDay);
    }

    protected void OnTanggalResponsChanged(DateTime? tanggalRespons)
    {
        ValidationModel!.NullableTanggalRespons = tanggalRespons;
        // LogSwitch.Debug("Tanggal respons: {0}", tanggalRespons!.Value.Date);
    }

    protected void OnLinkChatHistoryChanged(string linkChatHistory)
    {
        ValidationModel!.LinkChatHistory = linkChatHistory;
        // LogSwitch.Debug("Link chat history: {0}", linkChatHistory);
    }

    protected void OnKeteranganChanged(string keterangan)
    {
        ValidationModel!.Keterangan = keterangan;
        // LogSwitch.Debug("Keterangan: {0}", keterangan);
    }

    protected async Task OnPembetulanNamaChangedAsync(string pembetulanNama)
    {
        string namaPelanggan = ValidationCorrectionPropertyNames.PembetulanNama;
        ValidationModel!.PembetulanNama = pembetulanNama;

        await PembetulanProperty(namaPelanggan, pembetulanNama);
    }

    protected async Task OnPembetulanNomorTeleponChangedAsync(string pembetulanNomorTelepon)
    {
        string nomorTelepon = ValidationCorrectionPropertyNames.PembetulanNomorTelepon;
        ValidationModel!.PembetulanNomorTelepon = pembetulanNomorTelepon;

        await PembetulanProperty(nomorTelepon, pembetulanNomorTelepon);
    }

    protected async Task OnPembetulanEmailChangedAsync(string pembetulanEmail)
    {
        string email = ValidationCorrectionPropertyNames.PembetulanEmail;
        ValidationModel!.PembetulanEmail = pembetulanEmail;

        await PembetulanProperty(email, pembetulanEmail);
    }

    protected async Task OnPembetulanIdPlnChangedAsync(string pembetulanIdPln)
    {
        string idPln = ValidationCorrectionPropertyNames.PembetulanIdPln;
        ValidationModel!.PembetulanIdPln = pembetulanIdPln;

        await PembetulanProperty(idPln, pembetulanIdPln);
    }

    protected async Task OnPembetulanAlamatChangedAsync(string pembetulanAlamat)
    {
        string alamat = ValidationCorrectionPropertyNames.PembetulanAlamat;
        ValidationModel!.PembetulanAlamat = pembetulanAlamat;

        await PembetulanProperty(alamat, pembetulanAlamat);
    }

    private async Task UpdateProsesValidasi(WorkPaper workPaper, string broadcastMessage)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);
        LogSwitch.Debug("Broadcast validation {message}", broadcastMessage);
    }

    private void ClipboardToast(string clipboard)
    {
        var intent = ToastIntent.Info;
        var message = $"Copy: {clipboard}";
        var timeout = 2500; // milliseconds

        ToastService.ShowToast(intent, message, timeout: timeout);
    }

    private bool EnableCommitButton()
    {
        return ValidationModel?.ValidasiNama != OptionSelect.StatusValidasi.MenungguValidasi
            && ValidationModel?.ValidasiNomorTelepon != OptionSelect.StatusValidasi.MenungguValidasi
            && ValidationModel?.ValidasiEmail != OptionSelect.StatusValidasi.MenungguValidasi
            && ValidationModel?.ValidasiIdPln != OptionSelect.StatusValidasi.MenungguValidasi
            && ValidationModel?.ValidasiAlamat != OptionSelect.StatusValidasi.MenungguValidasi
            && ValidationModel?.ShareLoc != string.Empty;
    }

    private static Icon GetValidationIcon(string? section,
        string questionIconColor = "var(--info)",
        string errorIconColor = "var(--error-red)",
        string checkmarkIconColor = "var(--success)")
    {
        return section switch
        {
            string status when status == OptionSelect.StatusValidasi.MenungguValidasi => new Icons.Filled.Size20.QuestionCircle().WithColor(questionIconColor),
            string status when status == OptionSelect.StatusValidasi.TidakSesuai => new Icons.Filled.Size20.ErrorCircle().WithColor(errorIconColor),
            string status when status == OptionSelect.StatusValidasi.Sesuai => new Icons.Filled.Size20.CheckmarkCircle().WithColor(checkmarkIconColor),
            _ => throw new NotImplementedException(),
        };
    }

    private static string GetCssBackgroundColor(string? section,
        string waiting = "validation-value-bg-waiting",
        string invalid = "validation-value-bg-invalid",
        string valid = "validation-value-bg-valid")
    {
        var css = section switch
        {
            string status when status == OptionSelect.StatusValidasi.MenungguValidasi => waiting,
            string status when status == OptionSelect.StatusValidasi.TidakSesuai => invalid,
            string status when status == OptionSelect.StatusValidasi.Sesuai => valid,
            _ => throw new NotImplementedException(),
        };

        return $"{css} validation-value-clipboard";
    }

    [GeneratedRegex(RegexPattern.LatitudeLongitude)]
    private static partial Regex LatitudeLongitude();
}
