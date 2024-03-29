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

    protected string GetIdPln()
    {
        return WorkPaper!.ApprovalOpportunity.Pemohon.IdPln;
    }

    protected string GetAlamat()
    {
        return WorkPaper!.ApprovalOpportunity.Pemohon.Alamat;
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
}
