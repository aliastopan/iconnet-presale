using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadValidationForm : ComponentBase
{
    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeValidationModel")]
    public WorkloadValidationModel? ValidationModel { get; set; }

    private readonly Icon _question = new Icons.Filled.Size20.QuestionCircle().WithColor("var(--info)");
    private readonly Icon _error = new Icons.Filled.Size20.ErrorCircle().WithColor("var(--error)");
    private readonly Icon _checkmark = new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success)");

    protected IEnumerable<string> StatusValidasi => EnumHelper.GetIEnumerable<ValidationStatus>();

    protected string MenungguValidasi => StatusValidasi.First();
    protected string TidakSesuai => StatusValidasi.Skip(1).First();
    protected string Sesuai => StatusValidasi.Last();

    protected Func<string, bool> OptionDisableNamaPelanggan => x => x == MenungguValidasi && ValidationModel?.NamaPelanggan != MenungguValidasi;
    protected Func<string, bool> OptionDisableNoTelepon => x => x == MenungguValidasi && ValidationModel?.NomorTelepon != MenungguValidasi;
    protected Func<string, bool> OptionDisableEmail => x => x == MenungguValidasi && ValidationModel?.Email != MenungguValidasi;
    protected Func<string, bool> OptionDisableIdPln => x => x == MenungguValidasi && ValidationModel?.IdPln != MenungguValidasi;
    protected Func<string, bool> OptionDisableAlamat => x => x == MenungguValidasi && ValidationModel?.AlamatPelanggan != MenungguValidasi;

    protected Icon LabelIconNamaPelanggan => GetIcon(ValidationModel?.NamaPelanggan);
    protected Icon LabelIconNoTelepon => GetIcon(ValidationModel?.NomorTelepon);
    protected Icon LabelIconEmail => GetIcon(ValidationModel?.Email);
    protected Icon LabelIconIdPln => GetIcon(ValidationModel?.IdPln);
    protected Icon LabelIconAlamat => GetIcon(ValidationModel?.AlamatPelanggan);

    protected bool DisableTextFieldNamaPelanggan => ValidationModel?.NamaPelanggan != TidakSesuai;
    protected bool DisableTextFieldNoTelepon => ValidationModel?.NomorTelepon != TidakSesuai;
    protected bool DisableTextFieldEmail => ValidationModel?.Email != TidakSesuai;
    protected bool DisableTextFieldIdPln => ValidationModel?.IdPln != TidakSesuai;
    protected bool DisableTextAreaAlamatPelanggan => ValidationModel?.AlamatPelanggan != TidakSesuai;

    private Icon GetIcon(string? section)
    {
        return section switch
        {
            string status when status == TidakSesuai => _error,
            string status when status == Sesuai => _checkmark,
            _ => _question,
        };
    }

    public string? placeholder;
    public string? placeholderTextfield;
    public string? placeholderTextfield1;
    public string? placeholderTextfield2;
    public string? placeholderTextfield3;
    public string? placeholderTextfield4;

}
