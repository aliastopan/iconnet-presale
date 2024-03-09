namespace IConnet.Presale.WebApp.Models.Presales;

public class WorkPaperValidationModel
{
    public WorkPaperValidationModel(WorkPaper workPaper)
    {
        IsChatCallMulai = !workPaper.ProsesValidasi.SignatureChatCallMulai.IsEmptySignature();
        IdPermohonan = workPaper.ApprovalOpportunity.IdPermohonan;
        DataPelanggan = workPaper.ApprovalOpportunity.Pemohon;
        DataCrmKoordinat = workPaper.ApprovalOpportunity.Regional.Koordinat;

        ValidasiIdPln = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiIdPln);
        ValidasiNama = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNama);
        ValidasiNomorTelepon = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNomorTelepon);
        ValidasiEmail = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiEmail);
        ValidasiAlamat = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiAlamat);
        ValidasiCrmKoordinat = EnumProcessor.EnumToDisplayString(ValidationStatus.MenungguValidasi);

        PembetulanIdPln = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanIdPln;
        PembetulanNama = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanNama;
        PembetulanNomorTelepon = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanNomorTelepon;
        PembetulanEmail = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanEmail;
        PembetulanAlamat = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanAlamat;
    }

    public bool IsChatCallMulai { get; private set; }
    public string IdPermohonan { get; init; } = string.Empty;
    public Applicant DataPelanggan { get; init; } = new();
    public Coordinate DataCrmKoordinat { get; init; } = new();

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

    public DateTime GetWaktuTanggalRespons()
    {
        if (!NullableWaktuRespons.HasValue || !NullableTanggalRespons.HasValue)
        {
            throw new InvalidOperationException("Both NullableWaktuRespons and NullableTanggalRespons must have values.");
        }

        return new DateTime(NullableTanggalRespons.Value.Year,
            NullableTanggalRespons.Value.Month,
            NullableTanggalRespons.Value.Day,
            NullableWaktuRespons.Value.Hour,
            NullableWaktuRespons.Value.Minute,
            NullableWaktuRespons.Value.Second);
    }
}
