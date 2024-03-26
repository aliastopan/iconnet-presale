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

    protected bool IsLoading { get; set; } = false;
    protected bool IsCommitReady { get; set; } = false;

    protected bool DisableOnBeforeContact => !(ValidationModel?.IsChatCallMulai ?? false);
    protected bool DisableForms => DisableOnBeforeContact || !IsStillInCharge();

    protected Func<string, bool> OptionDisableNamaPelanggan => option => (option == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiNama != OptionSelect.StatusValidasi.MenungguValidasi)
        || !IsStillInCharge();
    protected Func<string, bool> OptionDisableNoTelepon => option => (option == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiNomorTelepon != OptionSelect.StatusValidasi.MenungguValidasi)
        || !IsStillInCharge();
    protected Func<string, bool> OptionDisableEmail => option => (option == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiEmail != OptionSelect.StatusValidasi.MenungguValidasi)
        || !IsStillInCharge();
    protected Func<string, bool> OptionDisableIdPln => option => (option == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiIdPln != OptionSelect.StatusValidasi.MenungguValidasi)
        || !IsStillInCharge();
    protected Func<string, bool> OptionDisableAlamat => option => (option == OptionSelect.StatusValidasi.MenungguValidasi
        && ValidationModel?.ValidasiAlamat != OptionSelect.StatusValidasi.MenungguValidasi)
        || !IsStillInCharge();

    protected Icon LabelIconNamaPelanggan => GetValidationIcon(ValidationModel?.ValidasiNama);
    protected Icon LabelIconNoTelepon => GetValidationIcon(ValidationModel?.ValidasiNomorTelepon);
    protected Icon LabelIconEmail => GetValidationIcon(ValidationModel?.ValidasiEmail);
    protected Icon LabelIconIdPln => GetValidationIcon(ValidationModel?.ValidasiIdPln);
    protected Icon LabelIconAlamat => GetValidationIcon(ValidationModel?.ValidasiAlamat);
    protected Icon LabelIconCrmKoordinat => GetValidationIcon(ValidationModel?.ValidasiCrmKoordinat, errorIconColor: "var(--info-grey)");

    protected string CssBackgroundColorNamaPelanggan => GetCssBackgroundColor(ValidationModel?.ValidasiNama);
    protected string CssBackgroundColorNomorTelepon => GetCssBackgroundColor(ValidationModel?.ValidasiNomorTelepon);
    protected string CssBackgroundColorEmail => GetCssBackgroundColor(ValidationModel?.ValidasiEmail);
    protected string CssBackgroundColorIdPln => GetCssBackgroundColor(ValidationModel?.ValidasiIdPln);
    protected string CssBackgroundColorAlamat => GetCssBackgroundColor(ValidationModel?.ValidasiAlamat);
    protected string CssBackgroundColorCrmKoordinat => GetCssBackgroundColor(ValidationModel?.ValidasiCrmKoordinat, invalid: "validation-value-bg-warning");

    protected bool DisableTextFieldNamaPelanggan => ValidationModel?.ValidasiNama != OptionSelect.StatusValidasi.TidakSesuai || !IsStillInCharge();
    protected bool DisableTextFieldNoTelepon => ValidationModel?.ValidasiNomorTelepon != OptionSelect.StatusValidasi.TidakSesuai || !IsStillInCharge();
    protected bool DisableTextFieldEmail => ValidationModel?.ValidasiEmail != OptionSelect.StatusValidasi.TidakSesuai || !IsStillInCharge();
    protected bool DisableTextFieldIdPln => ValidationModel?.ValidasiIdPln != OptionSelect.StatusValidasi.TidakSesuai || !IsStillInCharge();
    protected bool DisableTextAreaAlamatPelanggan => ValidationModel?.ValidasiAlamat != OptionSelect.StatusValidasi.TidakSesuai || !IsStillInCharge();

    protected bool DisableCommitToggle => !EnableCommitButton();
    protected bool DisableCommit => !IsCommitReady;

    protected async Task OnCommitAsync()
    {
        IsLoading = true;

        var coordinateShareLoc = new Coordinate(ValidationModel!.ShareLoc);

        DateTime waktuTanggalRespons = ValidationModel.GetWaktuTanggalRespons();
        DateTime validasiTimestamp = DateTimeService.DateTimeOffsetNow.DateTime;

        var signatureChatCallRespons = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = validasiTimestamp < waktuTanggalRespons
                ? waktuTanggalRespons.AddSeconds(30)
                : validasiTimestamp
        };

        var parameterValidasi = WorkPaper!.ProsesValidasi.ParameterValidasi.WithShareLoc(coordinateShareLoc);
        var prosesValidasi = WorkPaper.ProsesValidasi
            .WithParameterValidasi(parameterValidasi)
            .WithSignatureChatCallRespons(signatureChatCallRespons)
            .WithWaktuTanggalRespons(waktuTanggalRespons)
            .WithLinkChatHistory(ValidationModel.LinkChatHistory)
            .WithKeterangan(ValidationModel.Keterangan);

        WorkPaper.SetHelpdeskInCharge(signatureChatCallRespons);
        WorkPaper.ProsesValidasi = prosesValidasi;
        WorkPaper.WorkPaperLevel = WorkPaperLevel.WaitingApproval;
        WorkPaper.Shift = SessionService.GetShift();
        WorkPaper.LastModified = DateTimeService.DateTimeOffsetNow;

        var message = $"{signatureChatCallRespons.Alias} has commit chat/call validation to {WorkPaper.ApprovalOpportunity.IdPermohonan}";
        await UpdateProsesValidasiAsync(message);

        await UnstageWorkPaper.InvokeAsync();

        IsCommitReady = false;
        IsLoading = false;
    }

    protected async Task OnNotRespondingAsync()
    {
        var notRespondingSignature = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var prosesValidasi = WorkPaper!.ProsesValidasi
            .WithSignatureChatCallRespons(notRespondingSignature)
            .WithKeterangan(ValidationModel!.Keterangan);

        WorkPaper!.ProsesValidasi = prosesValidasi;
        WorkPaper!.WorkPaperLevel = WorkPaperLevel.WaitingApproval;

        var message = $"{WorkPaper.ApprovalOpportunity.IdPermohonan} is NOT RESPONDING";

        Task[] tasks =
        [
            UpdateProsesValidasiAsync(message),
            UnstageWorkPaper.InvokeAsync()
        ];

        await Task.WhenAll(tasks);
    }

    protected bool IsNotResponding()
    {
        DateTime today = DateTimeService.DateTimeOffsetNow.Date;

        return WorkPaper!.ProsesValidasi.IsNotResponding(today);
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

    protected async Task OpenChatTemplateDialogAsync()
    {
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
        if (!WorkPaper!.ProsesValidasi.SignatureChatCallMulai.IsEmptySignature())
        {
            return;
        }

        var signatureChatCallMulai = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };

        var prosesValidasi = WorkPaper!.ProsesValidasi.WithSignatureChatCallMulai(signatureChatCallMulai);

        WorkPaper.ProsesValidasi = prosesValidasi;

        var message = $"{signatureChatCallMulai.Alias} has began chat/call to {WorkPaper.ApprovalOpportunity.IdPermohonan}";
        await UpdateProsesValidasiAsync(message);

        ValidationModel!.MulaiChatCall();
    }

    protected async Task ValidasiProperty(string propertyName, string statusValidasi)
    {
        var validationStatus = EnumProcessor.StringToEnum<ValidationStatus>(statusValidasi);
        var parameterValidasi = WorkPaper!.ProsesValidasi.ParameterValidasi.Validasi(propertyName, validationStatus);
        var prosesValidasi = WorkPaper.ProsesValidasi.WithParameterValidasi(parameterValidasi);

        WorkPaper.ProsesValidasi = prosesValidasi;
        WorkPaper.LastModified = DateTimeService.DateTimeOffsetNow;

        var broadcastMessage = $"Validating: [{propertyName}:{WorkPaper.ProsesValidasi.ParameterValidasi.GetValidationStatus(propertyName)}]";
        await UpdateProsesValidasiAsync(broadcastMessage);
    }

    protected async Task PembetulanProperty(string propertyName, string pembetulan)
    {
        var pembetulanValidasi = WorkPaper!.ProsesValidasi.PembetulanValidasi.Pembetulan(propertyName, pembetulan);
        var prosesValidasi = WorkPaper.ProsesValidasi.WithPembetulanValidasi(pembetulanValidasi);

        WorkPaper.ProsesValidasi = prosesValidasi;
        WorkPaper.LastModified = DateTimeService.DateTimeOffsetNow;

        var broadcastMessage = $"Correction: [{propertyName}:{WorkPaper.ProsesValidasi.PembetulanValidasi.GetPembetulan(propertyName)}]";
        await UpdateProsesValidasiAsync(broadcastMessage);
    }

    protected async Task OnValidateNamaPelangganChangedAsync(string statusValidasi)
    {
        string namaPelanggan = ValidationParameterPropertyNames.ValidasiNama;
        ValidationModel!.ValidasiNama = statusValidasi;

        await ValidasiProperty(namaPelanggan, statusValidasi);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnValidateNomorTeleponChangedAsync(string statusValidasi)
    {
        string nomorTelepon = ValidationParameterPropertyNames.ValidasiNomorTelepon;
        ValidationModel!.ValidasiNomorTelepon = statusValidasi;

        await ValidasiProperty(nomorTelepon, statusValidasi);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnValidateEmailChangedAsync(string statusValidasi)
    {
        string email = ValidationParameterPropertyNames.ValidasiEmail;
        ValidationModel!.ValidasiEmail = statusValidasi;

        await ValidasiProperty(email, statusValidasi);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnValidateIdPlnChangedAsync(string statusValidasi)
    {
        string idPln = ValidationParameterPropertyNames.ValidasiIdPln;
        ValidationModel!.ValidasiIdPln = statusValidasi;

        await ValidasiProperty(idPln, statusValidasi);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnValidateAlamatChangedAsync(string statusValidasi)
    {
        string alamat = ValidationParameterPropertyNames.ValidasiAlamat;
        ValidationModel!.ValidasiAlamat = statusValidasi;

        await ValidasiProperty(alamat, statusValidasi);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected void OnShareLocChanged(string shareLoc)
    {
        if (!LatitudeLongitude().IsMatch(shareLoc))
        {
            ValidationModel!.ShareLoc = ValidationModel.ShareLoc;

            return;
        }

        ValidationModel!.ShareLoc = shareLoc;
        ValidationModel.ValidasiCrmKoordinat = ValidationModel!.ShareLoc.Equals(WorkPaper!.ApprovalOpportunity.Regional.Koordinat.GetLatitudeLongitude())
            ? OptionSelect.StatusValidasi.Sesuai
            : OptionSelect.StatusValidasi.TidakSesuai;

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected void OnWaktuResponsChanged(DateTime? waktuRespons)
    {
        // var priorValue = ValidationModel!.NullableWaktuRespons;

        ValidationModel!.NullableWaktuRespons = waktuRespons;

        if (ValidationModel!.GetWaktuTanggalRespons() < WorkPaper!.ProsesValidasi.SignatureChatCallMulai.TglAksi)
        {
            InvalidWaktuResponsToast();

            ValidationModel!.NullableWaktuRespons = DateTimeService.DateTimeOffsetNow.DateTime;
        }

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected void OnTanggalResponsChanged(DateTime? tanggalRespons)
    {
        var priorValue = ValidationModel!.NullableTanggalRespons;

        ValidationModel!.NullableTanggalRespons = tanggalRespons;

        if (ValidationModel!.GetWaktuTanggalRespons() < WorkPaper!.ProsesValidasi.SignatureChatCallMulai.TglAksi)
        {
            InvalidWaktuResponsToast();

            ValidationModel!.NullableTanggalRespons = priorValue;
        }

        if (ValidationModel!.GetWaktuTanggalRespons().Date > DateTimeService.DateTimeOffsetNow.Date)
        {
            ValidationModel!.NullableTanggalRespons = priorValue;
        }

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected void OnLinkChatHistoryChanged(string linkChatHistory)
    {
        ValidationModel!.LinkChatHistory = linkChatHistory;

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected void OnKeteranganChanged(string keterangan)
    {
        ValidationModel!.Keterangan = keterangan;

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnPembetulanNamaChangedAsync(string pembetulanNama)
    {
        string namaPelanggan = ValidationCorrectionPropertyNames.PembetulanNama;
        ValidationModel!.PembetulanNama = pembetulanNama;

        await PembetulanProperty(namaPelanggan, pembetulanNama);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnPembetulanNomorTeleponChangedAsync(string pembetulanNomorTelepon)
    {
        string nomorTelepon = ValidationCorrectionPropertyNames.PembetulanNomorTelepon;
        ValidationModel!.PembetulanNomorTelepon = pembetulanNomorTelepon;

        await PembetulanProperty(nomorTelepon, pembetulanNomorTelepon);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnPembetulanEmailChangedAsync(string pembetulanEmail)
    {
        string email = ValidationCorrectionPropertyNames.PembetulanEmail;
        ValidationModel!.PembetulanEmail = pembetulanEmail;

        await PembetulanProperty(email, pembetulanEmail);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnPembetulanIdPlnChangedAsync(string pembetulanIdPln)
    {
        string idPln = ValidationCorrectionPropertyNames.PembetulanIdPln;
        ValidationModel!.PembetulanIdPln = pembetulanIdPln;

        await PembetulanProperty(idPln, pembetulanIdPln);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnPembetulanAlamatChangedAsync(string pembetulanAlamat)
    {
        string alamat = ValidationCorrectionPropertyNames.PembetulanAlamat;
        ValidationModel!.PembetulanAlamat = pembetulanAlamat;

        await PembetulanProperty(alamat, pembetulanAlamat);

        if (!IsStillInCharge())
        {
            StagingExpiredToast();
        }
    }

    protected async Task OnOpenGoogleMapAsync()
    {
        var url = ValidationModel!.DataCrmKoordinat.GetGoogleMapLink();

        await JsRuntime.InvokeVoidAsync("open", url, "_blank");
    }

    private async Task UpdateProsesValidasiAsync(string broadcastMessage)
    {
        await WorkloadManager.UpdateWorkloadAsync(WorkPaper!);
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);
    }

    protected bool IsStillInCharge()
    {
        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = InChargeDuration.ValidationDuration;

        return !WorkPaper!.SignatureHelpdeskInCharge.IsDurationExceeded(now, duration);
    }

    protected async Task RestageWorkloadAsync()
    {
        WorkPaper!.SetHelpdeskInCharge(new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        });

        var broadcastMessage = "Extend staging duration";

        await UpdateProsesValidasiAsync(broadcastMessage);

        StagingExtendToast();
    }

    private void ClipboardToast(string clipboard)
    {
        var intent = ToastIntent.Info;
        var message = $"Copy: {clipboard}";
        var timeout = 3000; // milliseconds

        ToastService.ShowToast(intent, message, timeout: timeout);
    }

    private void StagingExpiredToast()
    {
        var intent = ToastIntent.Info;
        var message = "Masa tampung telah habis";
        var timeout = 3000; // milliseconds

        ToastService.ShowToast(intent, message, timeout: timeout);
    }

    private void StagingExtendToast()
    {
        var intent = ToastIntent.Success;
        var message = "Masa tampung telah berhasil diperpanjang";
        var timeout = 3000; // milliseconds

        ToastService.ShowToast(intent, message, timeout: timeout);
    }

    private void InvalidWaktuResponsToast()
    {
        var intent = ToastIntent.Warning;
        var message = "Waktu/Tanggal respons tidak bisa kurang dari Waktu/Tanggal Chat/Call Mulai";
        var timeout = 3000; // milliseconds

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
        string questionIconColor = "var(--info-grey)",
        string errorIconColor = "var(--error-red)",
        string checkmarkIconColor = "var(--success-green)")
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
