namespace IConnet.Presale.WebApp.Models.Presales;

public class WorkloadValidationModel
{
    public WorkloadValidationModel(WorkPaper workPaper)
    {
        IsChatCallMulai = !workPaper.ProsesValidasi.ChatCallMulai.IsEmptySignature();

        IdPermohonan = workPaper.ApprovalOpportunity.IdPermohonan;
        ValidasiIdPln = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiIdPln);
        ValidasiNama = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNama);
        ValidasiNomorTelepon = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNomorTelepon);
        ValidasiEmail = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiEmail);
        ValidasiAlamat = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiAlamat);
        ValidasiCrmKoordinat = EnumProcessor.EnumToDisplayString(ValidationStatus.MenungguValidasi);
    }

    public bool IsChatCallMulai { get; private set; }
    public string IdPermohonan { get; set; } = string.Empty;
    public string ValidasiIdPln { get; set; } = string.Empty;
    public string ValidasiNama { get; set; } = string.Empty;
    public string ValidasiNomorTelepon { get; set; } = string.Empty;
    public string ValidasiEmail { get; set; } = string.Empty;
    public string ValidasiAlamat { get; set; } = string.Empty;
    public string ValidasiCrmKoordinat { get; set; } = string.Empty;
    public string ShareLoc { get; set; } = string.Empty;
    public string LinkChatHistory { get; set; } = string.Empty;
    public string Keterangan { get; set; } = string.Empty;

    public DateTime? NullableWaktuRespons { get; set; } = DateTime.Today;
    public DateTime? NullableTanggalRespons { get; set; } = DateTime.Today;

    public string PembetulanIdPln { get; set; } = string.Empty;
    public string PembetulanNama { get; set; } = string.Empty;
    public string PembetulanNomorTelepon { get; set; } = string.Empty;
    public string PembetulanEmail { get; set; } = string.Empty;
    public string PembetulanAlamat { get; set; } = string.Empty;

    public void MulaiChatCall()
    {
        IsChatCallMulai = true;
    }
}
