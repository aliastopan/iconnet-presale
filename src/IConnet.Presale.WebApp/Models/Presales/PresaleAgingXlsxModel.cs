namespace IConnet.Presale.WebApp.Models.Presales;

public class PresaleAgingXlsxModel
{
    private readonly IntervalCalculatorService _intervalCalculatorService;

    public PresaleAgingXlsxModel(WorkPaper workPaper, IntervalCalculatorService intervalCalculatorService)
    {
        _intervalCalculatorService = intervalCalculatorService;

        IdPermohonan = workPaper.ApprovalOpportunity.IdPermohonan;
        TglPermohonan = workPaper.ApprovalOpportunity.TglPermohonan;

        PicImport = workPaper.ApprovalOpportunity.SignatureImport.Alias;
        TimestampImport = workPaper.ApprovalOpportunity.SignatureImport.TglAksi;
        AgingImport = GetAgingInterval(workPaper.ApprovalOpportunity.TglPermohonan, workPaper.ApprovalOpportunity.SignatureImport.TglAksi);

        PicVerifikasi = workPaper.ApprovalOpportunity.SignatureVerifikasiImport.Alias;
        TimestampVerifikasi = workPaper.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi;
        AgingVerifikasi = GetAgingInterval(workPaper.ApprovalOpportunity.SignatureImport.TglAksi, workPaper.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi);

        PicChatCallMulai = workPaper.ProsesValidasi.SignatureChatCallMulai.Alias;
        TimestampChatCallMulai = workPaper.ProsesValidasi.SignatureChatCallMulai.TglAksi;
        AgingChatCallMulai = GetAgingInterval(workPaper.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi, workPaper.ProsesValidasi.SignatureChatCallMulai.TglAksi);

        PicChatCallRespons = workPaper.ProsesValidasi.SignatureChatCallRespons.Alias;
        TimestampChatCallRespons = workPaper.ProsesValidasi.SignatureChatCallRespons.TglAksi;
        AgingChatCallRespons = GetAgingInterval(workPaper.ProsesValidasi.WaktuTanggalRespons, workPaper.ProsesValidasi.SignatureChatCallRespons.TglAksi);

        PicApproval = workPaper.ProsesApproval.SignatureApproval.Alias;
        TimestampApproval = workPaper.ProsesApproval.SignatureApproval.TglAksi;
        AgingApproval = GetAgingInterval(workPaper.ProsesValidasi.SignatureChatCallRespons.TglAksi, workPaper.ProsesApproval.SignatureApproval.TglAksi);
    }

    public string IdPermohonan { get; init; }
    public DateTime TglPermohonan { get; init; }

    public string PicImport { get; init; }
    public DateTime TimestampImport { get; init; }
    public TimeSpan AgingImport { get; init; }

    public string PicVerifikasi { get; init; }
    public DateTime TimestampVerifikasi { get; init; }
    public TimeSpan AgingVerifikasi { get; init; }

    public string PicChatCallMulai { get; init; }
    public DateTime TimestampChatCallMulai { get; init; }
    public TimeSpan AgingChatCallMulai { get; init; }

    public string PicChatCallRespons { get; init; }
    public DateTime TimestampChatCallRespons { get; init; }
    public TimeSpan AgingChatCallRespons { get; init; }

    public string PicApproval { get; init; }
    public DateTime TimestampApproval { get; init; }
    public TimeSpan AgingApproval { get; init; }

    private TimeSpan GetAgingInterval(DateTime start, DateTime end)
    {
        return _intervalCalculatorService.CalculateInterval(start, end, excludeFrozenInterval: true);
    }
}
