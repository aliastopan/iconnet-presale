using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkPaperApprovalForm : ComponentBase
{
    [Parameter]
    public EventCallback UnstageWorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeApprovalModel")]
    public WorkPaperApprovalModel? ApprovalModel { get; set; }

    private readonly Icon _questionIcon = new Icons.Filled.Size20.QuestionCircle();
    private readonly Icon _errorIcon = new Icons.Filled.Size20.ErrorCircle();
    private readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

    protected Icon LabelIconNamaPelanggan => GetIcon(ApprovalModel?.ValidasiNama.ToString());


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
}
