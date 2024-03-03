using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkPaperApprovalForm : ComponentBase
{
    [Inject] public OptionService OptionService { get; set; } = default!;

    [Parameter]
    public EventCallback UnstageWorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeWorkPaper")]
    public WorkPaper? WorkPaper { get; set; }

    [CascadingParameter(Name = "CascadeApprovalModel")]
    public WorkPaperApprovalModel? ApprovalModel { get; set; }

    private static readonly Icon _questionIcon = new Icons.Filled.Size20.QuestionCircle();
    private static readonly Icon _errorIcon = new Icons.Filled.Size20.ErrorCircle();
    private static readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

    protected Func<string, bool> OptionDisableNamaOnProgress => option => option == OptionSelect.StatusApproval.OnProgress
        && ApprovalModel!.StatusApproval != OptionSelect.StatusApproval.OnProgress;

    protected bool DisableRootCause => ApprovalModel!.StatusApproval != OptionSelect.StatusApproval.Reject
        && ApprovalModel!.StatusApproval != OptionSelect.StatusApproval.ClosedLost;

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

    protected string CssStyleStrikethroughNamaPelanggan => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiNama);
    protected string CssStyleStrikethroughNomorTelepon => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiNomorTelepon);
    protected string CssStyleStrikethroughEmail => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiEmail);
    protected string CssStyleStrikethroughIdPln => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiIdPln);
    protected string CssStyleStrikethroughAlamat => GetCssStyleStrikethrough(ApprovalModel!.HasilValidasi.ValidasiAlamat);

    protected string GetShareLoc()
    {
        return ApprovalModel!.HasilValidasi.ShareLoc.LatitudeLongitude;
    }

    protected string GetKeteranganValidasi()
    {
        return ApprovalModel!.KeteranganValidasi;
    }

    protected async Task OnJarakShareLocAsync(string jarakShareLoc)
    {
        if (!jarakShareLoc.IsParsableAsInteger())
        {
            return;
        }

        ApprovalModel!.JarakShareLoc = jarakShareLoc;

        LogSwitch.Debug("Jarak ShareLoc: {0}", ApprovalModel!.JarakShareLoc);

        await Task.CompletedTask;
    }

    protected async Task OnJarakICrmAsync(string jarakICrmPlus)
    {
        if (!jarakICrmPlus.IsParsableAsInteger())
        {
            return;
        }

        ApprovalModel!.JarakICrmPlus = jarakICrmPlus;

        LogSwitch.Debug("Jarak iCRM: {0}", ApprovalModel!.JarakICrmPlus);

        await Task.CompletedTask;
    }

    protected void OnVaTerbit(DateTime? tanggalVaTerbit)
    {
        ApprovalModel!.NullableVaTerbit = tanggalVaTerbit;
        LogSwitch.Debug("Va Terbit: {0}", tanggalVaTerbit!.Value.Date);
    }

    protected async Task OnStatusApprovalAsync(string statusApproval)
    {
        ApprovalModel!.StatusApproval = statusApproval;

        if (ApprovalModel!.RootCause.IsNullOrWhiteSpace())
        {
            ApprovalModel!.RootCause = OptionService.RootCauseOptions.First();
        }

        await Task.CompletedTask;
    }

    protected async Task OnRootCauseAsync(string rootCause)
    {
        ApprovalModel!.RootCause = rootCause;

        await Task.CompletedTask;
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
        string info = "approval-value-bg-info",
        string valid = "approval-value-bg-valid")
    {
        switch (section)
        {
            case ValidationStatus.Sesuai:
                return valid;
            default:
                return info;
        }
    }

    private static string GetCssStyleStrikethrough(ValidationStatus validationStatus)
    {
        var style = validationStatus == ValidationStatus.Sesuai
            ? string.Empty
            : "text-decoration: line-through; color: #ababab !important;";

        return style;
    }
}
