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

    private readonly Icon _questionIcon = new Icons.Filled.Size20.QuestionCircle();
    private readonly Icon _errorIcon = new Icons.Filled.Size20.ErrorCircle();
    private readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

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
    protected bool DisableCommit => !EnableCommitButton();

    protected async Task OnCommitAsync()
    {
        if (WorkPaper is null || ValidationModel is null)
        {
            return;
        }

        var coordinateShareLoc = new Coordinate(ValidationModel.ShareLoc);
        var signatureChatCallRespons = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var parameterValidasi = WorkPaper.ProsesValidasi.ParameterValidasi.WithShareLoc(coordinateShareLoc);
        var prosesValidasi = WorkPaper.ProsesValidasi
            .WithParameterValidasi(parameterValidasi)
            .WithSignatureChatCallRespons(signatureChatCallRespons)
            .WithWaktuTanggalRespons(ValidationModel.GetWaktuTanggalRespons())
            .WithLinkRekapChatHistory(ValidationModel.LinkRekapChatHistory)
            .WithKeterangan(ValidationModel.Keterangan);

        WorkPaper.ProsesValidasi = prosesValidasi;
        WorkPaper.WorkPaperLevel = WorkPaperLevel.WaitingApproval;

        var message = $"{signatureChatCallRespons.Alias} has commit chat/call validation to {WorkPaper.ApprovalOpportunity.IdPermohonan}";
        await UpdateProsesValidasi(WorkPaper, message);

        await UnstageWorkPaper.InvokeAsync();
    }

    protected async Task OnClosedLostAsync()
    {
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

    protected string GetAgingResponseString()
    {
        DateTime today = DateTimeService.DateTimeOffsetNow.Date;
        TimeSpan agingResponse = WorkPaper!.ProsesValidasi.GetAgingChatCallRespons(today);

        if (agingResponse.TotalHours < 1)
        {
            return "kurang dari 1 Jam";
        }
        else
        {
            int totalDays = (int)agingResponse.TotalDays;
            int hours = agingResponse.Hours;

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
        string latitudeLongitude = ValidationModel!.DataCrmKoordinat.LatitudeLongitude;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", latitudeLongitude);
        ClipboardToast(latitudeLongitude);

        LogSwitch.Debug("Copying {0}", latitudeLongitude);
    }

    protected void OpenDialog()
    {
        LogSwitch.Debug("Chat Template Dialog");
    }

    protected async Task OpenDialogAsync()
    {
        if (WorkPaper is null)
        {
            return;
        }

        LogSwitch.Debug("Chat Template Dialog");

        var parameters = new DialogParameters()
        {
            Title = "Chat Template",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<ChatTemplateDialog>(WorkPaper, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }
    }

    protected async Task UpdateChatCallMulaiSignature()
    {
        if (WorkPaper is null || ValidationModel is null)
        {
            return;
        }

        var signatureChatCallMulai = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var prosesValidasi = WorkPaper.ProsesValidasi.WithSignatureChatCallMulai(signatureChatCallMulai);

        WorkPaper.ProsesValidasi = prosesValidasi;

        var message = $"{signatureChatCallMulai.Alias} has began chat/call to {WorkPaper.ApprovalOpportunity.IdPermohonan}";
        await UpdateProsesValidasi(WorkPaper, message);

        ValidationModel.MulaiChatCall();
    }

    protected async Task ValidasiProperty(string propertyName, string statusValidasi)
    {
        if (WorkPaper is null || ValidationModel is null)
        {
            return;
        }

        var validationStatus = EnumProcessor.StringToEnum<ValidationStatus>(statusValidasi);
        var parameterValidasi = WorkPaper.ProsesValidasi.ParameterValidasi.Validasi(propertyName, validationStatus);
        var prosesValidasi = WorkPaper.ProsesValidasi.WithParameterValidasi(parameterValidasi);

        WorkPaper.ProsesValidasi = prosesValidasi;

        var broadcastMessage = $"Validating: [{propertyName}:{WorkPaper.ProsesValidasi.ParameterValidasi.GetValidationStatus(propertyName)}]";
        await UpdateProsesValidasi(WorkPaper, broadcastMessage);
    }

    protected async Task PembetulanProperty(string propertyName, string pembetulan)
    {
        if (WorkPaper is null || ValidationModel is null)
        {
            return;
        }

        var pembetulanValidasi = WorkPaper.ProsesValidasi.PembetulanValidasi.Pembetulan(propertyName, pembetulan);
        var prosesValidasi = WorkPaper.ProsesValidasi.WithPembetulanValidasi(pembetulanValidasi);

        WorkPaper.ProsesValidasi = prosesValidasi;

        var broadcastMessage = $"Correction: [{propertyName}:{WorkPaper.ProsesValidasi.PembetulanValidasi.GetPembetulan(propertyName)}]";
        await UpdateProsesValidasi(WorkPaper, broadcastMessage);
    }

    protected async Task OnValidateNamaPelangganAsync(string statusValidasi)
    {
        string namaPelanggan = ValidationParameterPropertyNames.ValidasiNama;
        ValidationModel!.ValidasiNama = statusValidasi;

        await ValidasiProperty(namaPelanggan, statusValidasi);
    }

    protected async Task OnValidateNomorTeleponAsync(string statusValidasi)
    {
        string nomorTelepon = ValidationParameterPropertyNames.ValidasiNomorTelepon;
        ValidationModel!.ValidasiNomorTelepon = statusValidasi;

        await ValidasiProperty(nomorTelepon, statusValidasi);
    }

    protected async Task OnValidateEmailAsync(string statusValidasi)
    {
        string email = ValidationParameterPropertyNames.ValidasiEmail;
        ValidationModel!.ValidasiEmail = statusValidasi;

        await ValidasiProperty(email, statusValidasi);
    }

    protected async Task OnValidateIdPlnAsync(string statusValidasi)
    {
        string idPln = ValidationParameterPropertyNames.ValidasiIdPln;
        ValidationModel!.ValidasiIdPln = statusValidasi;

        await ValidasiProperty(idPln, statusValidasi);
    }

    protected async Task OnValidateAlamatAsync(string statusValidasi)
    {
        string alamat = ValidationParameterPropertyNames.ValidasiAlamat;
        ValidationModel!.ValidasiAlamat = statusValidasi;

        await ValidasiProperty(alamat, statusValidasi);
    }

    protected void OnShareLoc(string shareLoc)
    {
        if (ValidationModel is null)
        {
            return;
        }


        if (!LatitudeLongitude().IsMatch(shareLoc))
        {
            ValidationModel.ShareLoc = ValidationModel.ShareLoc;
            LogSwitch.Debug("Invalid share loc format: {0}", shareLoc);

            return;
        }

        ValidationModel.ShareLoc = shareLoc;
        ValidationModel.ValidasiCrmKoordinat = ValidationModel!.ShareLoc.Equals(WorkPaper!.ApprovalOpportunity.Regional.Koordinat.LatitudeLongitude)
            ? OptionSelect.StatusValidasi.Sesuai
            : OptionSelect.StatusValidasi.TidakSesuai;

        LogSwitch.Debug("Share loc: {0}", shareLoc);
    }

    protected void OnWaktuRespons(DateTime? waktuRespons)
    {
        if (ValidationModel is null)
        {
            return;
        }

        ValidationModel.NullableWaktuRespons = waktuRespons;
        LogSwitch.Debug("Waktu respons: {0}", waktuRespons!.Value.TimeOfDay);
    }

    protected void OnTanggalRespons(DateTime? tanggalRespons)
    {
        if (ValidationModel is null)
        {
            return;
        }

        ValidationModel.NullableTanggalRespons = tanggalRespons;
        LogSwitch.Debug("Tanggal respons: {0}", tanggalRespons!.Value.Date);
    }

    protected void OnLinkRekapChatHistory(string linkChatHistory)
    {
        if (ValidationModel is null)
        {
            return;
        }

        ValidationModel.LinkRekapChatHistory = linkChatHistory;
        LogSwitch.Debug("Link chat history: {0}", linkChatHistory);
    }

    protected void OnKeterangan(string keterangan)
    {
        if (ValidationModel is null)
        {
            return;
        }

        ValidationModel.Keterangan = keterangan;
        LogSwitch.Debug("Keterangan: {0}", keterangan);
    }

    protected async Task OnPembetulanNamaAsync(string pembetulanNama)
    {
        string namaPelanggan = ValidationCorrectionPropertyNames.PembetulanNama;
        ValidationModel!.PembetulanNama = pembetulanNama;

        await PembetulanProperty(namaPelanggan, pembetulanNama);
    }

    protected async Task OnPembetulanNomorTeleponAsync(string pembetulanNomorTelepon)
    {
        string nomorTelepon = ValidationCorrectionPropertyNames.PembetulanNomorTelepon;
        ValidationModel!.PembetulanNomorTelepon = pembetulanNomorTelepon;

        await PembetulanProperty(nomorTelepon, pembetulanNomorTelepon);
    }

    protected async Task OnPembetulanEmailAsync(string pembetulanEmail)
    {
        string email = ValidationCorrectionPropertyNames.PembetulanEmail;
        ValidationModel!.PembetulanEmail = pembetulanEmail;

        await PembetulanProperty(email, pembetulanEmail);
    }

    protected async Task OnPembetulanIdPlnAsync(string pembetulanIdPln)
    {
        string idPln = ValidationCorrectionPropertyNames.PembetulanIdPln;
        ValidationModel!.PembetulanIdPln = pembetulanIdPln;

        await PembetulanProperty(idPln, pembetulanIdPln);
    }

    protected async Task OnPembetulanAlamatAsync(string pembetulanAlamat)
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

    private Icon GetValidationIcon(string? section,
        string questionIconColor = "var(--info)",
        string errorIconColor = "var(--error)",
        string checkmarkIconColor = "var(--success)")
    {
        return section switch
        {
            string status when status == OptionSelect.StatusValidasi.TidakSesuai => _errorIcon.WithColor(errorIconColor),
            string status when status == OptionSelect.StatusValidasi.Sesuai => _checkmarkIcon.WithColor(checkmarkIconColor),
            _ => _questionIcon.WithColor(questionIconColor),
        };
    }

    private static string GetCssBackgroundColor(string? section,
        string waiting = "validation-value-bg-waiting",
        string invalid = "validation-value-bg-invalid",
        string valid = "validation-value-bg-valid")
    {
        var css = section switch
        {
            string status when status == OptionSelect.StatusValidasi.TidakSesuai => invalid,
            string status when status == OptionSelect.StatusValidasi.Sesuai => valid,
            _ => waiting,
        };

        return $"{css} validation-value-clipboard";
    }

    [GeneratedRegex(RegexPattern.LatitudeLongitude)]
    private static partial Regex LatitudeLongitude();
}
