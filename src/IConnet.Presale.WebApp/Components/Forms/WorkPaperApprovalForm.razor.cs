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

    private static readonly Icon _questionIcon = new Icons.Filled.Size20.QuestionCircle();
    private static readonly Icon _errorIcon = new Icons.Filled.Size20.ErrorCircle();
    private static readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

    protected Icon LabelIconNamaPelanggan => GetIcon(ApprovalModel!.ValidasiNama);
    protected Icon LabelIconNoTelepon => GetIcon(ApprovalModel!.ValidasiNomorTelepon);
    protected Icon LabelIconEmail => GetIcon(ApprovalModel!.ValidasiEmail);
    protected Icon LabelIconIdPln => GetIcon(ApprovalModel!.ValidasiIdPln);
    protected Icon LabelIconAlamat => GetIcon(ApprovalModel!.ValidasiAlamat);

    private static Icon GetIcon(ValidationStatus section,
        string questionIconColor = "var(--info)",
        string errorIconColor = "var(--warning)",
        string checkmarkIconColor = "var(--success)")
    {
        switch (section)
        {
            case ValidationStatus.TidakSesuai:
                return _errorIcon.WithColor(errorIconColor);
            case ValidationStatus.Sesuai:
                return _checkmarkIcon.WithColor(checkmarkIconColor);
            default:
                return _questionIcon.WithColor(questionIconColor);
        }
    }
}
