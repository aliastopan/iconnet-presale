using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadValidationForm : ComponentBase
{
    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeValidationModel")]
    public WorkloadValidationModel? ValidationModel { get; set; }

    public IEnumerable<string> StatusValidasi => EnumHelper.GetIEnumerable<ValidationStatus>();

    public string TidakSesuai => StatusValidasi.Skip(1).First();
    public bool DisableNamaPelanggan => ValidationModel?.NamaPelanggan != TidakSesuai;
    public bool DisableNoTelepon => ValidationModel?.NomorTelepon != TidakSesuai;
    public bool DisableEmail => ValidationModel?.Email != TidakSesuai;
    public bool DisableIdPln => ValidationModel?.IdPln != TidakSesuai;
    public bool DisableAlamatPelanggan => ValidationModel?.AlamatPelanggan != TidakSesuai;

    public string? placeholder;
    public string? placeholderTextfield;
    public string? placeholderTextfield1;
    public string? placeholderTextfield2;
    public string? placeholderTextfield3;
    public string? placeholderTextfield4;

}
