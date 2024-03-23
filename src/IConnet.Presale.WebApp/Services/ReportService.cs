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
            var count = presaleData.Count(x => x.ProsesApproval.StatusApproval == approvalStatus);
            statusPerOffice.Add(count);
        }

        return new ApprovalStatusReportModel(approvalStatus, offices, statusPerOffice);
    }
}
