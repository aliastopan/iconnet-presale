namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadValidationForm
{
    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    public IEnumerable<string> StatusValidasi => EnumHelper.GetIEnumerable<ValidationStatus>(skipFirst: true);

    public string? placeholder;
    public string? placeholderTextfield;
    public string? placeholderTextfield1;
    public string? placeholderTextfield2;
    public string? placeholderTextfield3;
    public string? placeholderTextfield4;

}
