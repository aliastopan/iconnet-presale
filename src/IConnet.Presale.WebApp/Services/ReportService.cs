using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public class ReportService
{
    private readonly OptionService _optionService;

    public ReportService(OptionService optionService)
    {
        _optionService = optionService;
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
}
