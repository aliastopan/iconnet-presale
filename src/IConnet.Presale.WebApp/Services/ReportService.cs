using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public class ReportService
{
    private readonly OptionService _optionService;
    private readonly IntervalCalculatorService _intervalCalculatorService;

    public ReportService(OptionService optionService,
        IntervalCalculatorService intervalCalculatorService)
    {
        _optionService = optionService;
        _intervalCalculatorService = intervalCalculatorService;
    }

    public ApprovalStatusReportModel GenerateApprovalStatusReport(ApprovalStatus approvalStatus,
        IQueryable<WorkPaper> presaleData)
    {
        List<string> offices = _optionService.KantorPerwakilanOptions.Skip(1).ToList();
        List<int> statusPerOffice = [];

        for (int i = 0; i < offices.Count; i++)
        {
            int count = presaleData.Count(x => x.ProsesApproval.StatusApproval == approvalStatus
                && x.ApprovalOpportunity.Regional.KantorPerwakilan.Equals(offices[i], StringComparison.OrdinalIgnoreCase));

            statusPerOffice.Add(count);
        }

        return new ApprovalStatusReportModel(approvalStatus, offices, statusPerOffice);
    }

    public RootCauseReportModel GenerateRootCauseReport(string rootCause,
        IQueryable<WorkPaper> presaleData)
    {
        List<string> offices = _optionService.KantorPerwakilanOptions.Skip(1).ToList();
        List<int> rootCausePerOffice = [];

        for (int i = 0; i < offices.Count; i++)
        {
            int count = presaleData.Count(x => (x.ProsesApproval.StatusApproval == ApprovalStatus.Reject
                || x.ProsesApproval.StatusApproval == ApprovalStatus.CloseLost)
                && x.ProsesApproval.RootCause.Equals(rootCause, StringComparison.OrdinalIgnoreCase)
                && x.ApprovalOpportunity.Regional.KantorPerwakilan.Equals(offices[i], StringComparison.OrdinalIgnoreCase));

            rootCausePerOffice.Add(count);
        }

        return new RootCauseReportModel(rootCause, offices, rootCausePerOffice);
    }

    public ImportAgingReportModel? GenerateImportAgingReport(PresaleOperatorModel presaleOperator, IQueryable<WorkPaper> presaleData)
    {
        if (presaleOperator.UserRole != UserRole.PAC)
        {
            return null;
        }

        List<TimeSpan> agingIntervals = [];

        foreach (var data in presaleData)
        {
            if (data.ApprovalOpportunity.SignatureImport.AccountIdSignature != presaleOperator.UserAccountId)
            {
                continue;
            }

            DateTime startDateTime = data.ApprovalOpportunity.TglPermohonan;
            DateTime endDateTime = data.ApprovalOpportunity.SignatureImport.TglAksi;

            TimeSpan interval = _intervalCalculatorService.CalculateInterval(startDateTime, endDateTime, excludeFrozenInterval: true);

            agingIntervals.Add(interval);
        }

        TimeSpan avg, max, min;

        if (agingIntervals.Count > 0)
        {
            avg = GetAverageTimeSpan(agingIntervals);
            max = agingIntervals.Max();
            min = agingIntervals.Min();
        }
        else
        {
            avg = TimeSpan.Zero;
            max = TimeSpan.Zero;
            min = TimeSpan.Zero;
        }

        int importCount = agingIntervals.Count;

        var pacId = presaleOperator.UserAccountId;
        var username = presaleOperator.Username;


        return new ImportAgingReportModel(pacId, username, avg, min, max, importCount);
    }

    public AgingReportModel GenerateAgingImportReport(IQueryable<WorkPaper> presaleData)
    {
        List<TimeSpan> agingReport = [];

        foreach (var data in presaleData)
        {
            DateTime start = data.ApprovalOpportunity.TglPermohonan;
            DateTime end = data.ApprovalOpportunity.SignatureImport.TglAksi;

            TimeSpan interval = _intervalCalculatorService.CalculateInterval(start, end, excludeFrozenInterval: true);

            agingReport.Add(interval);
        }

        var avg = GetAverageTimeSpan(agingReport);
        var max = agingReport.Max();
        var min = agingReport.Min();

        return new AgingReportModel(avg, min, max);
    }

    private static TimeSpan GetAverageTimeSpan(List<TimeSpan> agingReport)
    {
        if (agingReport == null || agingReport.Count == 0)
        {
            return TimeSpan.Zero;
        }

        long totalTicks = agingReport.Sum(ts => ts.Ticks);
        long averageTicks = totalTicks / agingReport.Count;

        return new TimeSpan(averageTicks);
    }
}
