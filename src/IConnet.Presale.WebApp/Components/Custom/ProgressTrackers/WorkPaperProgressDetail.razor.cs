using System.Globalization;

namespace IConnet.Presale.WebApp.Components.Custom.ProgressTrackers;

public partial class WorkPaperProgressDetail
{
    [Parameter]
    public WorkPaper? WorkPaper { get; set; }

    protected bool IsSplitterGanti => WorkPaper!.ProsesApproval.IsSplitterGanti();
    protected bool HasShareLoc => !WorkPaper!.ProsesValidasi.ParameterValidasi.ShareLoc.IsEmptyCoordinate();
    protected bool IsApproved => WorkPaper!.ProsesApproval.StatusApproval == ApprovalStatus.Approve;

    protected string GetIdPermohonan()
    {
        return WorkPaper!.ApprovalOpportunity.IdPermohonan;
    }

    protected string GetNamaPemohon()
    {
        return WorkPaper!.ApprovalOpportunity.Pemohon.NamaPelanggan;
    }

    protected string GetNoTelp()
    {
        return WorkPaper!.ApprovalOpportunity.Pemohon.NomorTelepon;
    }

    protected string GetEmail()
    {
        return WorkPaper!.ApprovalOpportunity.Pemohon.Email;
    }

    protected string GetIdPln()
    {
        return WorkPaper!.ApprovalOpportunity.Pemohon.IdPln;
    }

    protected string GetAlamat()
    {
        return WorkPaper!.ApprovalOpportunity.Pemohon.Alamat;
    }

    protected string GetNamaAgen()
    {
        return WorkPaper!.ApprovalOpportunity.Agen.NamaLengkap;
    }

    protected string GetKantorPerwakilan()
    {
        return WorkPaper!.ApprovalOpportunity.Regional.KantorPerwakilan;
    }

    protected string GetSplitter()
    {
        return WorkPaper!.ApprovalOpportunity.Splitter;
    }

    protected string GetSplitterGanti()
    {
        return WorkPaper!.ProsesApproval.SplitterGanti;
    }

    protected int GetJarakShareLoc()
    {
        return WorkPaper!.ProsesApproval.JarakShareLoc;
    }

    protected int GetJarakICrmPlus()
    {
        return WorkPaper!.ProsesApproval.JarakICrmPlus;
    }

    protected string GetShareLoc()
    {
        return WorkPaper!.ProsesValidasi.ParameterValidasi.ShareLoc.GetLatitudeLongitude();
    }

    protected string GetGoogleMapLink()
    {
        return WorkPaper!.ProsesValidasi.ParameterValidasi.ShareLoc.GetGoogleMapLink();
    }

    protected string GetVaTerbit()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return WorkPaper!.ProsesApproval.VaTerbit.ToString("dd MMM yyyy", cultureInfo);
    }

    protected string GetBadgeColorStyle()
    {
        switch (WorkPaper!.ProsesApproval.StatusApproval)
        {
            case ApprovalStatus.CloseLost:
                return "background-color: var(--soft-black) !important;";
            case ApprovalStatus.Reject:
                return "background-color: var(--error-red) !important;";
            case ApprovalStatus.Approve:
                return "background-color: var(--success-green) !important;";
            default:
                return "";
        }
    }
}
