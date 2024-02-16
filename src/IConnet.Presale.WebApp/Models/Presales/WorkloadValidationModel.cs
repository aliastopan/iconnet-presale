namespace IConnet.Presale.WebApp.Models.Presales;

public class WorkloadValidationModel
{
    public WorkloadValidationModel(WorkPaper workPaper)
    {
        IdPermohonan = workPaper.ApprovalOpportunity.IdPermohonan;
        IdPln = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiIdPln);
        NamaPelanggan = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNama);
        NomorTelepon = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNomorTelepon);
        Email = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiEmail);
        AlamatPelanggan = EnumHelper.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiAlamat);
        CrmKoordinat = workPaper.ApprovalOpportunity.Regional.Koordinat.LatitudeLongitude;

        // PembetulanIdPln = IdPln;
        // PembetulanNamaPelanggan = NamaPelanggan;
        // PembetulanNomorTelepon = NomorTelepon;
        // PembetulanEmail = Email;
        // PembetulanAlamatPelanggan = AlamatPelanggan;
    }

    public string IdPermohonan { get; set; } = string.Empty;
    public string IdPln { get; set; } = string.Empty;
    public string NamaPelanggan { get; set; } = string.Empty;
    public string NomorTelepon { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AlamatPelanggan { get; set; } = string.Empty;
    public string CrmKoordinat { get; set; } = string.Empty;
    public string ShareLoc { get; set; } = string.Empty;

    public string PembetulanIdPln { get; set; } = string.Empty;
    public string PembetulanNamaPelanggan { get; set; } = string.Empty;
    public string PembetulanNomorTelepon { get; set; } = string.Empty;
    public string PembetulanEmail { get; set; } = string.Empty;
    public string PembetulanAlamatPelanggan { get; set; } = string.Empty;
}
