using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadValidationForm : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeValidationModel")]
    public WorkloadValidationModel? ValidationModel { get; set; }

    private readonly Icon _questionIcon = new Icons.Filled.Size20.QuestionCircle();
    private readonly Icon _errorIcon = new Icons.Filled.Size20.ErrorCircle();
    private readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

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

    protected Icon LabelIconNamaPelanggan => GetIcon(ValidationModel?.ValidasiNama);
    protected Icon LabelIconNoTelepon => GetIcon(ValidationModel?.ValidasiNomorTelepon);
    protected Icon LabelIconEmail => GetIcon(ValidationModel?.ValidasiEmail);
    protected Icon LabelIconIdPln => GetIcon(ValidationModel?.ValidasiIdPln);
    protected Icon LabelIconAlamat => GetIcon(ValidationModel?.ValidasiAlamat);
    protected Icon LabelIconCrmKoordinat => GetIcon(ValidationModel?.ValidasiCrmKoordinat, errorIconColor: "var(--warning)");

    protected string BgColorNamaPelanggan => GetBackgroundColor(ValidationModel?.ValidasiNama);
    protected string BgColorNomorTelepon => GetBackgroundColor(ValidationModel?.ValidasiNomorTelepon);
    protected string BgColorEmail => GetBackgroundColor(ValidationModel?.ValidasiEmail);
    protected string BgColorIdPln => GetBackgroundColor(ValidationModel?.ValidasiIdPln);
    protected string BgColorAlamat => GetBackgroundColor(ValidationModel?.ValidasiAlamat);
    protected string BgColorCrmKoordinat => GetBackgroundColor(ValidationModel?.ValidasiCrmKoordinat, invalid: "validation-value-bg-warning");

    protected bool DisableTextFieldNamaPelanggan => ValidationModel?.ValidasiNama != OptionSelect.StatusValidasi.TidakSesuai;
    protected bool DisableTextFieldNoTelepon => ValidationModel?.ValidasiNomorTelepon != OptionSelect.StatusValidasi.TidakSesuai;
    protected bool DisableTextFieldEmail => ValidationModel?.ValidasiEmail != OptionSelect.StatusValidasi.TidakSesuai;
    protected bool DisableTextFieldIdPln => ValidationModel?.ValidasiIdPln != OptionSelect.StatusValidasi.TidakSesuai;
    protected bool DisableTextAreaAlamatPelanggan => ValidationModel?.ValidasiAlamat != OptionSelect.StatusValidasi.TidakSesuai;

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

        var chatCallMulai = new ActionSignature
        {
            AccountIdSignature = SessionService.UserModel!.UserAccountId,
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var prosesValidasi = WorkPaper.ProsesValidasi.WithChatCallMulai(chatCallMulai);

        WorkPaper.ProsesValidasi = prosesValidasi;

        var message = $"{chatCallMulai.Alias} has began chat/call to {WorkPaper.ApprovalOpportunity.IdPermohonan}";
        await UpdateProsesValidasi(WorkPaper, message);
    }

    protected async Task ValidasiProperty(string propertyName, string statusValidasi)
    {
        if (WorkPaper is null || ValidationModel is null)
        {
            return;
        }

        var validationStatus = EnumProcessor.StringToEnum<ValidationStatus>(statusValidasi);
        var parameterValidasi = WorkPaper.ProsesValidasi.ParameterValidasi.Validate(propertyName, validationStatus);
        var prosesValidasi = WorkPaper.ProsesValidasi.WithParameterValidasi(parameterValidasi);

        WorkPaper.ProsesValidasi = prosesValidasi;
        LogSwitch.Debug($"Validasi {propertyName}: {statusValidasi}");

        var broadcastMessage = $"[{propertyName}:{WorkPaper.ProsesValidasi.ParameterValidasi.GetValidationStatus(propertyName)}]";
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

    protected void OnLinkChatHistory(string linkChatHistory)
    {
        if (ValidationModel is null)
        {
            return;
        }

        ValidationModel.LinkChatHistory = linkChatHistory;
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

    private async Task UpdateProsesValidasi(WorkPaper workPaper, string broadcastMessage)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);
        await BroadcastService.BroadcastMessageAsync($"Validating '{broadcastMessage}'");
        LogSwitch.Debug("Broadcast validation {message}", broadcastMessage);
    }

    private Icon GetIcon(string? section,
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

    private static string GetBackgroundColor(string? section,
        string waiting = "validation-value-bg-waiting",
        string invalid = "validation-value-bg-invalid",
        string valid = "validation-value-bg-valid")
    {
        return section switch
        {
            string status when status == OptionSelect.StatusValidasi.TidakSesuai => invalid,
            string status when status == OptionSelect.StatusValidasi.Sesuai => valid,
            _ => waiting,
        };
    }
}
