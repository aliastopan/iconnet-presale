using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadValidationForm : ComponentBase
{
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;

    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeValidationModel")]
    public WorkloadValidationModel? ValidationModel { get; set; }

    private readonly Icon _questionIcon = new Icons.Filled.Size20.QuestionCircle();
    private readonly Icon _errorIcon = new Icons.Filled.Size20.ErrorCircle();
    private readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

    protected IEnumerable<string> StatusValidasiOptions => EnumProcessor.GetStringOptions<ValidationStatus>();

    protected string MenungguValidasi => StatusValidasiOptions.First();
    protected string TidakSesuai => StatusValidasiOptions.Skip(1).First();
    protected string Sesuai => StatusValidasiOptions.Last();

    protected Func<string, bool> OptionDisableNamaPelanggan => x => x == MenungguValidasi && ValidationModel?.ValidasiNama != MenungguValidasi;
    protected Func<string, bool> OptionDisableNoTelepon => x => x == MenungguValidasi && ValidationModel?.ValidasiNomorTelepon != MenungguValidasi;
    protected Func<string, bool> OptionDisableEmail => x => x == MenungguValidasi && ValidationModel?.ValidasiEmail != MenungguValidasi;
    protected Func<string, bool> OptionDisableIdPln => x => x == MenungguValidasi && ValidationModel?.ValidasiIdPln != MenungguValidasi;
    protected Func<string, bool> OptionDisableAlamat => x => x == MenungguValidasi && ValidationModel?.ValidasiAlamat != MenungguValidasi;

    protected Icon LabelIconNamaPelanggan => GetIcon(ValidationModel?.ValidasiNama);
    protected Icon LabelIconNoTelepon => GetIcon(ValidationModel?.ValidasiNomorTelepon);
    protected Icon LabelIconEmail => GetIcon(ValidationModel?.ValidasiEmail);
    protected Icon LabelIconIdPln => GetIcon(ValidationModel?.ValidasiIdPln);
    protected Icon LabelIconAlamat => GetIcon(ValidationModel?.ValidasiAlamat);
    protected Icon LabelIconShareLoc=> GetIcon(ValidationModel?.ValidasiCrmKoordinat, errorIconColor: "var(--warning)");

    protected string BgColorNamaPelanggan => GetBackgroundColor(ValidationModel?.ValidasiNama);
    protected string BgColorNoTelepon => GetBackgroundColor(ValidationModel?.ValidasiNomorTelepon);
    protected string BgColorEmail => GetBackgroundColor(ValidationModel?.ValidasiEmail);
    protected string BgColorIdPln => GetBackgroundColor(ValidationModel?.ValidasiIdPln);
    protected string BgColorAlamat => GetBackgroundColor(ValidationModel?.ValidasiAlamat);

    protected bool DisableTextFieldNamaPelanggan => ValidationModel?.ValidasiNama != TidakSesuai;
    protected bool DisableTextFieldNoTelepon => ValidationModel?.ValidasiNomorTelepon != TidakSesuai;
    protected bool DisableTextFieldEmail => ValidationModel?.ValidasiEmail != TidakSesuai;
    protected bool DisableTextFieldIdPln => ValidationModel?.ValidasiIdPln != TidakSesuai;
    protected bool DisableTextAreaAlamatPelanggan => ValidationModel?.ValidasiAlamat != TidakSesuai;

    protected void OnShareLoc(string shareLoc)
    {
        if (ValidationModel is null)
        {
            return;
        }

        ValidationModel.ShareLoc = shareLoc;
        ValidationModel.ValidasiCrmKoordinat = ValidationModel!.ShareLoc.Equals(WorkPaper!.ApprovalOpportunity.Regional.Koordinat.LatitudeLongitude)
            ? Sesuai
            : TidakSesuai;
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

    protected async Task OnValidateNamaPelanggan(string statusValidasi)
    {
        if (WorkPaper is null|| ValidationModel is null)
        {
            return;
        }

        ValidationModel.ValidasiNama = statusValidasi;

        var validasiNama = EnumProcessor.StringToEnum<ValidationStatus>(statusValidasi);
        var parameterValidasi = WorkPaper.ProsesValidasi.ParameterValidasi.WithValidasiNama(validasiNama);
        var prosesValidasi =  WorkPaper.ProsesValidasi.WithParameterValidasi(parameterValidasi);

        WorkPaper.ProsesValidasi = prosesValidasi;
        LogSwitch.Debug("Validasi Name: {statusValidasi}", validasiNama);

        await UpdateProsesValidasi(WorkPaper, $"[NamaPelanggan:{WorkPaper.ProsesValidasi.ParameterValidasi.ValidasiNama}]");
    }

    private async Task UpdateProsesValidasi(WorkPaper workPaper, string message)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);
        await BroadcastService.BroadcastMessageAsync($"Validating '{message}'");
        LogSwitch.Debug("Broadcast validation {message}", message);
    }

    private Icon GetIcon(string? section,
        string questionIconColor = "var(--info)",
        string errorIconColor = "var(--error)",
        string checkmarkIconColor = "var(--success)")
    {
        return section switch
        {
            string status when status == TidakSesuai => _errorIcon.WithColor(errorIconColor),
            string status when status == Sesuai => _checkmarkIcon.WithColor(checkmarkIconColor),
            _ => _questionIcon.WithColor(questionIconColor),
        };
    }

    private string GetBackgroundColor(string? section)
    {
        return section switch
        {
            string status when status == TidakSesuai => "validation-value-bg-invalid",
            string status when status == Sesuai => "validation-value-bg-valid",
            _ => "validation-value-bg-waiting",
        };
    }
}
