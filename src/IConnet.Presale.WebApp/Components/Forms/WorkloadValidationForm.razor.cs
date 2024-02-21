using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadValidationForm : ComponentBase
{
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeValidationModel")]
    public WorkloadValidationModel? ValidationModel { get; set; }

    private readonly Icon _question = new Icons.Filled.Size20.QuestionCircle().WithColor("var(--info)");
    private readonly Icon _error = new Icons.Filled.Size20.ErrorCircle().WithColor("var(--error)");
    private readonly Icon _checkmark = new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success)");

    protected IEnumerable<string> StatusValidasi => EnumProcessor.GetIEnumerable<ValidationStatus>();

    protected string MenungguValidasi => StatusValidasi.First();
    protected string TidakSesuai => StatusValidasi.Skip(1).First();
    protected string Sesuai => StatusValidasi.Last();

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
    protected Icon LabelIconShareLoc=> GetIcon(ValidationModel?.ValidasiCrmKoordinat);

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

    private Icon GetIcon(string? section)
    {
        return section switch
        {
            string status when status == TidakSesuai => _error,
            string status when status == Sesuai => _checkmark,
            _ => _question,
        };
    }
}
