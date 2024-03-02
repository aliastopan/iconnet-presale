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

        ValidasiIdPln = workPaper.ProsesValidasi.ParameterValidasi.ValidasiIdPln;
        ValidasiNama = workPaper.ProsesValidasi.ParameterValidasi.ValidasiNama;
        ValidasiNomorTelepon = workPaper.ProsesValidasi.ParameterValidasi.ValidasiNomorTelepon;
        ValidasiEmail = workPaper.ProsesValidasi.ParameterValidasi.ValidasiEmail;
        ValidasiAlamat = workPaper.ProsesValidasi.ParameterValidasi.ValidasiAlamat;
        ShareLoc = workPaper.ProsesValidasi.ParameterValidasi.ShareLoc;
        KeteranganValidasi = workPaper.ProsesValidasi.Keterangan;

        PembetulanIdPln = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanIdPln;
        PembetulanNama = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanNama;
        PembetulanNomorTelepon = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanNomorTelepon;
        PembetulanEmail = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanEmail;
        PembetulanAlamat = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanAlamat;
    }

    public string IdPermohonan { get; set; } = string.Empty;
    public string StatusApproval { get; set; } = string.Empty;
    public string RootCause { get; set; } = string.Empty;
    public string Keterangan { get; set; } = string.Empty;
    public string JarakShareLoc { get; set; } = string.Empty;
    public string JarakICrmPlus { get; set; } = string.Empty;
    public DateTime? NullableVaTerbit { get; set; } = DateTime.Today;

    public ValidationStatus ValidasiIdPln { get; set; }
    public ValidationStatus ValidasiNama { get; set; }
    public ValidationStatus ValidasiNomorTelepon { get; set; }
    public ValidationStatus ValidasiEmail { get; set; }
    public ValidationStatus ValidasiAlamat { get; set; }
    public Coordinate ShareLoc { get; set; } = new();
    public string KeteranganValidasi { get; set; } = string.Empty;

    public string PembetulanIdPln { get; set; } = string.Empty;
    public string PembetulanNama { get; set; } = string.Empty;
    public string PembetulanNomorTelepon { get; set; } = string.Empty;
    public string PembetulanEmail { get; set; } = string.Empty;
    public string PembetulanAlamat { get; set; } = string.Empty;
}
