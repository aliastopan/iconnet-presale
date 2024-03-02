namespace IConnet.Presale.WebApp.Models.Presales;

public class WorkPaperApprovalModel
{
    public WorkPaperApprovalModel(WorkPaper workPaper)
    {
        IdPermohonan = workPaper.ApprovalOpportunity.IdPermohonan;
        DataPelanggan = workPaper.ApprovalOpportunity.Pemohon;
        DataPembetulan = workPaper.ProsesValidasi.PembetulanValidasi;
        HasilValidasi = workPaper.ProsesValidasi.ParameterValidasi;
        KeteranganValidasi = workPaper.ProsesValidasi.Keterangan;

        StatusApproval = EnumProcessor.EnumToDisplayString(workPaper.ProsesApproval.StatusApproval);
        RootCause = workPaper.ProsesApproval.RootCause;
        Keterangan = workPaper.ProsesApproval.Keterangan;
        JarakShareLoc = workPaper.ProsesApproval.JarakShareLoc;
        JarakICrmPlus = workPaper.ProsesApproval.JarakICrmPlus;
    }

    public string IdPermohonan { get; init; } = string.Empty;
    public Applicant DataPelanggan { get; init; } = new();
    public ValidationCorrection DataPembetulan { get; init; } = new();
    public ValidationParameter HasilValidasi { get; init; } = new();
    public string KeteranganValidasi { get; init; } = string.Empty;

    public string StatusApproval { get; set; } = string.Empty;
    public string RootCause { get; set; } = string.Empty;
    public string Keterangan { get; set; } = string.Empty;
    public string JarakShareLoc { get; set; } = string.Empty;
    public string JarakICrmPlus { get; set; } = string.Empty;
    public DateTime? NullableVaTerbit { get; set; } = DateTime.Today;
}
