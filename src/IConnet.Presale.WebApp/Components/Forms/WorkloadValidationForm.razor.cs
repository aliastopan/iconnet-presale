using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadValidationForm : ComponentBase
{
    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeValidationModel")]
    public WorkloadValidationModel? ValidationModel { get; set; }

    private Icon _question = new Icons.Filled.Size20.QuestionCircle().WithColor("var(--info)");
    private Icon _error = new Icons.Filled.Size20.ErrorCircle().WithColor("var(--error)");
    private Icon _checkmark = new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success)");

    protected IEnumerable<string> StatusValidasi => EnumHelper.GetIEnumerable<ValidationStatus>();

    protected string MenungguValidasi => StatusValidasi.First();
    protected string TidakSesuai => StatusValidasi.Skip(1).First();
    protected string Sesuai => StatusValidasi.Last();

    protected Icon LabelIconNamaPelanggan => GetIcon(ValidationModel?.NamaPelanggan);
    protected Icon LabelIconNoTelepon => GetIcon(ValidationModel?.NomorTelepon);
    protected Icon LabelIconEmail => GetIcon(ValidationModel?.Email);
    protected Icon LabelIconIdPln => GetIcon(ValidationModel?.IdPln);
    protected Icon LabelIconAlamat => GetIcon(ValidationModel?.AlamatPelanggan);

    protected bool DisableNamaPelanggan => ValidationModel?.NamaPelanggan != TidakSesuai;
    protected bool DisableNoTelepon => ValidationModel?.NomorTelepon != TidakSesuai;
    protected bool DisableEmail => ValidationModel?.Email != TidakSesuai;
    protected bool DisableIdPln => ValidationModel?.IdPln != TidakSesuai;
    protected bool DisableAlamatPelanggan => ValidationModel?.AlamatPelanggan != TidakSesuai;

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
