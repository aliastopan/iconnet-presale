namespace IConnet.Presale.WebApp.Models.Presales;

public class WorkloadValidationModel
{
    public WorkloadValidationModel(WorkPaper workPaper)
    {
        IdPermohonan = workPaper.ApprovalOpportunity.IdPermohonan;
        ValidasiIdPln = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiIdPln);
        ValidasiNama = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNama);
        ValidasiNomorTelepon = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNomorTelepon);
        ValidasiEmail = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiEmail);
        ValidasiAlamat = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiAlamat);
        ValidasiCrmKoordinat = EnumHelper.EnumToDisplayString(ValidationStatus.MenungguValidasi);
    }

    public string IdPermohonan { get; set; } = string.Empty;
    public string ValidasiIdPln { get; set; } = string.Empty;
    public string ValidasiNama { get; set; } = string.Empty;
    public string ValidasiNomorTelepon { get; set; } = string.Empty;
    public string ValidasiEmail { get; set; } = string.Empty;
    public string ValidasiAlamat { get; set; } = string.Empty;
    public string ValidasiCrmKoordinat { get; set; } = string.Empty;
    public string ShareLoc { get; set; } = string.Empty;

    public string PembetulanIdPln { get; set; } = string.Empty;
    public string PembetulanNama { get; set; } = string.Empty;
    public string PembetulanNomorTelepon { get; set; } = string.Empty;
    public string PembetulanEmail { get; set; } = string.Empty;
    public string PembetulanAlamat { get; set; } = string.Empty;
}
