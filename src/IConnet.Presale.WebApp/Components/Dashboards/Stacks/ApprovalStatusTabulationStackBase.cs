using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ApprovalStatusTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected ReportService ReportService { get; set;} = default!;

    [Parameter] public List<ApprovalStatusReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> LowerBoundaryModels { get; set; } = [];

    public List<ApprovalStatusTransposeModel> UpperBoundaryTransposeModels => ReportService.TransposeModel(UpperBoundaryModels);
    public List<ApprovalStatusTransposeModel> MiddleBoundaryTransposeModels => ReportService.TransposeModel(MiddleBoundaryModels);
    public List<ApprovalStatusTransposeModel> LowerBoundaryTransposeModels => ReportService.TransposeModel(LowerBoundaryModels);

    public Dictionary<string, List<ApprovalStatusReportModel>> UpperBoundaryGrouping => BoundaryGrouping(UpperBoundaryModels);

    public static Dictionary<string, List<ApprovalStatusReportModel>> BoundaryGrouping(List<ApprovalStatusReportModel> boundaryModels)
    {
        var boundaryModelGroups = new Dictionary<string, List<ApprovalStatusReportModel>>();

        var availableOffices = boundaryModels.SelectMany(m => m.StatusPerOffice.Keys).Distinct().ToList();
        var reportModelGroups = boundaryModels.GroupBy(m => m.ApprovalStatus);

        foreach (var model in reportModelGroups)
        {
            var approvalStatus = model.Key;

            foreach (var office in availableOffices)
            {
                var officeTotal = model.Sum(m => m.StatusPerOffice.GetValueOrDefault(office, 0));

                var statusPerOffice = new Dictionary<string, int> { { office, officeTotal } };
                var reportModel = new ApprovalStatusReportModel(approvalStatus, statusPerOffice);

                if (!boundaryModelGroups.ContainsKey(office))
                {
                    boundaryModelGroups[office] = new List<ApprovalStatusReportModel>();
                }

                boundaryModelGroups[office].Add(reportModel);
            }
        }

        return boundaryModelGroups;
    }
}
