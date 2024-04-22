using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Services;

public class SlaService
{
    private TimeSpan _slaImport;
    private TimeSpan _slaPickUp;
    private TimeSpan _slaValidasi;
    private TimeSpan _slaApproval;

    public SlaService(IConfiguration configuration)
    {
        string slaImportString = configuration["ServiceLevelAgreement:Import"]!;
        string slaPickUpString = configuration["ServiceLevelAgreement:PickUp"]!;
        string slaValidasiString = configuration["ServiceLevelAgreement:Validasi"]!;
        string slaApprovalString = configuration["ServiceLevelAgreement:Approval"]!;

        _slaImport = TimeSpan.Parse(slaImportString);
        _slaPickUp = TimeSpan.Parse(slaPickUpString);
        _slaValidasi = TimeSpan.Parse(slaValidasiString);
        _slaApproval = TimeSpan.Parse(slaApprovalString);
    }

    public string GetSlaImportVerdict(Guid operatorId, List<ImportAgingReportModel> Models)
    {
        var importAging = Models.Where(x => x.PacId == operatorId);
        var isWinning = importAging.Any(x => x.Average < _slaImport);

        return isWinning
            ? "Win"
            : "Lose";
    }
}
