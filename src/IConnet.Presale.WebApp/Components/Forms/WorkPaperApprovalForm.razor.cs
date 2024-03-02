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

    protected Icon LabelIconNamaPelanggan => GetIcon(ApprovalModel!.HasilValidasi.ValidasiNama);
    protected Icon LabelIconNoTelepon => GetIcon(ApprovalModel!.HasilValidasi.ValidasiNomorTelepon);
    protected Icon LabelIconEmail => GetIcon(ApprovalModel!.HasilValidasi.ValidasiEmail);
    protected Icon LabelIconIdPln => GetIcon(ApprovalModel!.HasilValidasi.ValidasiIdPln);
    protected Icon LabelIconAlamat => GetIcon(ApprovalModel!.HasilValidasi.ValidasiAlamat);

    protected string CssBackgroundColorNamaPelanggan => GetCssBackgroundColor(ApprovalModel!.HasilValidasi.ValidasiNama);
    protected string CssBackgroundColorNomorTelepon => GetCssBackgroundColor(ApprovalModel!.HasilValidasi.ValidasiNomorTelepon);
    protected string CssBackgroundColorEmail => GetCssBackgroundColor(ApprovalModel!.HasilValidasi.ValidasiEmail);
    protected string CssBackgroundColorIdPln => GetCssBackgroundColor(ApprovalModel!.HasilValidasi.ValidasiIdPln);
    protected string CssBackgroundColorAlamat => GetCssBackgroundColor(ApprovalModel!.HasilValidasi.ValidasiAlamat);

    public string GetNamaPelanggan()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiNama != ValidationStatus.Sesuai)
        {
            return ApprovalModel!.DataPembetulan.PembetulanNama;
        }

        return ApprovalModel!.DataPelanggan.NamaLengkap;
    }

    public string GetNomorTelepon()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiNomorTelepon != ValidationStatus.Sesuai)
        {
            return ApprovalModel!.DataPembetulan.PembetulanNomorTelepon;
        }

        return ApprovalModel!.DataPelanggan.NomorTelepon;
    }

    public string GetEmail()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiEmail != ValidationStatus.Sesuai)
        {
            return ApprovalModel!.DataPembetulan.PembetulanEmail;
        }

        return ApprovalModel!.DataPelanggan.Email;
    }

    public string GetIdPln()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiIdPln != ValidationStatus.Sesuai)
        {
            return ApprovalModel!.DataPembetulan.PembetulanIdPln;
        }

        return ApprovalModel!.DataPelanggan.IdPln;
    }

    public string GetAlamat()
    {
        if (ApprovalModel!.HasilValidasi.ValidasiAlamat != ValidationStatus.Sesuai)
        {
            return ApprovalModel!.DataPembetulan.PembetulanAlamat;
        }

        return ApprovalModel!.DataPelanggan.Alamat;
    }

    public string GetShareLoc()
    {
        return ApprovalModel!.HasilValidasi.ShareLoc.LatitudeLongitude;
    }

    public string GetKeteranganValidasi()
    {
        return ApprovalModel!.KeteranganValidasi;
    }

    private static Icon GetIcon(ValidationStatus section,
        string questionIconColor = "var(--info)",
        string errorIconColor = "var(--error)",
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

    private static string GetCssBackgroundColor(ValidationStatus section,
        string waiting = "approval-value-bg-waiting",
        string invalid = "approval-value-bg-invalid",
        string valid = "approval-value-bg-valid")
    {
        switch (section)
        {
            case ValidationStatus.TidakSesuai:
                return invalid;
            case ValidationStatus.Sesuai:
                return valid;
            default:
                return waiting;
        }
    }
}
