namespace IConnet.Presale.WebApp.Models.Presales;

public class WorkPaperApprovalModel
{
    public WorkPaperApprovalModel(WorkPaper workPaper)
    {
        IdPermohonan = workPaper.ApprovalOpportunity.IdPermohonan;
    }

    public string IdPermohonan { get; set; } = string.Empty;
}
