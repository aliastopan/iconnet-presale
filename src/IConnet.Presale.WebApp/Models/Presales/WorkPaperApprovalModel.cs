namespace IConnet.Presale.WebApp.Models.Presales;

public class WorkPaperApprovalModel
{
    public WorkPaperApprovalModel(WorkPaper workPaper)
    {
        IdPermohonan = workPaper.ApprovalOpportunity.IdPermohonan;

        StatusApproval = EnumProcessor.EnumToDisplayString(workPaper.ProsesApproval.StatusApproval);
        RootCause = workPaper.ProsesApproval.RootCause;
        Keterangan = workPaper.ProsesApproval.Keterangan;
        JarakShareLoc = workPaper.ProsesApproval.JarakShareLoc;
        JarakICrmPlus = workPaper.ProsesApproval.JarakICrmPlus;
    }

    public string IdPermohonan { get; set; } = string.Empty;
    public string StatusApproval { get; init; } = string.Empty;
    public string RootCause { get; init; } = string.Empty;
    public string Keterangan { get; init; } = string.Empty;
    public string JarakShareLoc { get; init; } = string.Empty;
    public string JarakICrmPlus { get; init; } = string.Empty;
    public DateTime? NullableVaTerbit { get; init; } = DateTime.Today;
}
