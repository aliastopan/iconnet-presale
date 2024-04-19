using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales.Reports;

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

    public CustomerResponseAgingReport GenerateCustomerResponseAgingReport(IQueryable<WorkPaper> presaleData)
    {
        List<TimeSpan> agingIntervals = [];

        foreach (var data in presaleData)
        {
            bool isDoneProcessing = data.WorkPaperLevel == WorkPaperLevel.DoneProcessing;

            if(!isDoneProcessing)
            {
                continue;
            }

            DateTime startDateTime = data.ProsesValidasi.SignatureChatCallMulai.TglAksi;
            DateTime endDateTime = data.ProsesValidasi.WaktuTanggalRespons;

            TimeSpan interval = _intervalCalculatorService.CalculateInterval(startDateTime, endDateTime, excludeFrozenInterval: false);

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

        return new CustomerResponseAgingReport(avg, min, max);
    }

    public ImportAgingReportModel? GenerateImportAgingReport(PresaleOperatorModel presaleOperator,
        IQueryable<WorkPaper> presaleData)
    {
        if (presaleOperator.UserRole != UserRole.PAC)
        {
            return null;
        }

        List<TimeSpan> agingIntervals = [];

        foreach (var data in presaleData)
        {
            bool matchInChargeSignature = data.ApprovalOpportunity.SignatureImport.AccountIdSignature == presaleOperator.UserAccountId;

            if (!matchInChargeSignature)
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

        int importTotal = agingIntervals.Count;

        var pacId = presaleOperator.UserAccountId;
        var username = presaleOperator.Username;

        return new ImportAgingReportModel(pacId, username, avg, min, max, importTotal);
    }

    public VerificationAgingReportModel? GenerateVerificationAgingReport(PresaleOperatorModel presaleOperator,
        IQueryable<WorkPaper> presaleData)
    {
        if (presaleOperator.UserRole != UserRole.PAC)
        {
            return null;
        }

        List<TimeSpan> agingIntervals = [];
        int verificationTotal = 0;
        int totalReject = 0;

        foreach (var data in presaleData)
        {
            bool matchInChargeSignature = data.ApprovalOpportunity.SignatureImport.AccountIdSignature == presaleOperator.UserAccountId;
            bool isDataPastVerification = !data.ApprovalOpportunity.SignatureVerifikasiImport.IsEmptySignature();

            if (!matchInChargeSignature || !isDataPastVerification)
            {
                continue;
            }

            verificationTotal++;

            if (data.WorkPaperLevel == WorkPaperLevel.ImportInvalid)
            {
                totalReject++;
                continue;
            }

            DateTime startDateTime = data.ApprovalOpportunity.SignatureImport.TglAksi;
            DateTime endDateTime = data.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi;

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

        int totalVerified = agingIntervals.Count;

        var pacId = presaleOperator.UserAccountId;
        var username = presaleOperator.Username;

        return new VerificationAgingReportModel(pacId, username, avg, min, max,
            verificationTotal, totalReject, totalVerified);
    }

    public ChatCallMulaiAgingReportModel? GenerateChatCallMulaiAgingReport(PresaleOperatorModel presaleOperator,
        IQueryable<WorkPaper> presaleData)
    {
        if (presaleOperator.UserRole != UserRole.Helpdesk)
        {
            return null;
        }

        List<TimeSpan> agingIntervals = [];

        foreach (var data in presaleData)
        {
            bool matchInChargeSignature = data.ProsesValidasi.SignatureChatCallMulai.AccountIdSignature == presaleOperator.UserAccountId;

            if (!matchInChargeSignature)
            {
                continue;
            }

            DateTime startDateTime = data.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi;
            DateTime endDateTime = data.ProsesValidasi.SignatureChatCallMulai.TglAksi;

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

        int chatCallMulaiTotal = agingIntervals.Count;

        var helpdeskId = presaleOperator.UserAccountId;
        var username = presaleOperator.Username;

        return new ChatCallMulaiAgingReportModel(helpdeskId, username, avg, min, max, chatCallMulaiTotal);
    }

    public ChatCallResponsAgingReportModel? GenerateChatCallResponsAgingReport(PresaleOperatorModel presaleOperator,
        IQueryable<WorkPaper> presaleData)
    {
        if (presaleOperator.UserRole != UserRole.Helpdesk)
        {
            return null;
        }

        List<TimeSpan> agingIntervals = [];

        foreach (var data in presaleData)
        {
            bool matchInChargeSignature = data.ProsesValidasi.SignatureChatCallRespons.AccountIdSignature == presaleOperator.UserAccountId;

            if (!matchInChargeSignature)
            {
                continue;
            }

            DateTime startDateTime = data.ProsesValidasi.WaktuTanggalRespons;
            DateTime endDateTime = data.ProsesValidasi.SignatureChatCallRespons.TglAksi;

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

        int chatCallResponsTotal = agingIntervals.Count;

        var helpdeskId = presaleOperator.UserAccountId;
        var username = presaleOperator.Username;

        return new ChatCallResponsAgingReportModel(helpdeskId, username, avg, min, max, chatCallResponsTotal);
    }

    public ApprovalAgingReportModel? GenerateApprovalAgingReport(PresaleOperatorModel presaleOperator,
        IQueryable<WorkPaper> presaleData)
    {
        if (presaleOperator.UserRole != UserRole.PAC)
        {
            return null;
        }

        List<TimeSpan> agingIntervals = [];
        int totalCloseLost = 0;
        int totalReject = 0;
        int totalExpansion = 0;
        int totalApprove = 0;
        int totalDirectApproval = 0;

        foreach (var data in presaleData)
        {
            bool matchInChargeSignature = data.ProsesApproval.SignatureApproval.AccountIdSignature == presaleOperator.UserAccountId;
            bool isDoneProcessing = data.WorkPaperLevel == WorkPaperLevel.DoneProcessing;
            bool isPendingExpansion = data.ProsesApproval.StatusApproval == ApprovalStatus.Expansion;
            bool isDirectlyApproved = data.ProsesApproval.IsDirectlyApproved();

            if (!matchInChargeSignature)
            {
                continue;
            }

            if (isDirectlyApproved)
            {
                // using aging verification as direct approval calculation
                // DateTime _startDateTime = data.ApprovalOpportunity.SignatureImport.TglAksi;
                // DateTime _endDateTime = data.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi;
                // TimeSpan _interval = _intervalCalculatorService.CalculateInterval(_startDateTime, _endDateTime, excludeFrozenInterval: true);

                TimeSpan fixedInterval = new TimeSpan(0, 10, 0);

                agingIntervals.Add(fixedInterval);
                totalDirectApproval++;

                continue;
            }

            if (!isDoneProcessing && !isPendingExpansion)
            {
                continue;
            }

            if (data.ProsesApproval.StatusApproval == ApprovalStatus.CloseLost)
            {
                totalCloseLost++;
            }

            if (data.ProsesApproval.StatusApproval == ApprovalStatus.Reject)
            {
                totalReject++;
            }

            if (data.ProsesApproval.StatusApproval == ApprovalStatus.Expansion)
            {
                totalExpansion++;
            }

            if (data.ProsesApproval.StatusApproval == ApprovalStatus.Approve)
            {
                totalApprove++;
            }

            DateTime startDateTime = data.ProsesValidasi.SignatureChatCallRespons.TglAksi;
            DateTime endDateTime = data.ProsesApproval.SignatureApproval.TglAksi;

            // handle (bug) not responding user
            if (data.ProsesValidasi.SignatureChatCallRespons.IsEmptySignature())
            {
                int notRespondingThreshold = 2;

                startDateTime = data.ProsesValidasi.SignatureChatCallMulai.TglAksi.AddDays(notRespondingThreshold);
            }

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

        int approvalTotal = agingIntervals.Count;

        var pacId = presaleOperator.UserAccountId;
        var username = presaleOperator.Username;

        return new ApprovalAgingReportModel(pacId, username, avg, min, max,
            approvalTotal, totalCloseLost, totalReject, totalExpansion, totalApprove, totalDirectApproval);
    }

    public List<ApprovalStatusTransposeModel> TransposeModel(List<ApprovalStatusReportModel> models)
    {
        return models.SelectMany(model => model.StatusPerOffice.Select(status => new
            {
                model.ApprovalStatus,
                Office = status.Key,
                Count = status.Value
            }))
            .GroupBy(x => x.Office)
            .Select(group => new ApprovalStatusTransposeModel
            {
                Office = group.Key,
                ApprovalStatusMetrics = group.ToDictionary(x => x.ApprovalStatus, x => x.Count)
            })
            .ToList();
    }

    public List<RootCauseReportTransposeModel> TransposeModel(List<RootCauseReportModel> models)
    {
        return models.SelectMany(model => model.RootCausePerOffice.Select(rootCause => new
            {
                model.RootCause,
                Office = rootCause.Key,
                Count = rootCause.Value
            }))
            .GroupBy(x => x.Office)
            .Select(group => new RootCauseReportTransposeModel
            {
                Office = group.Key,
                RootCauseMetrics = group.ToDictionary(x => x.RootCause, x => x.Count)
            })
            .ToList();
    }

    public Dictionary<string, List<ApprovalStatusReportModel>> ApprovalStatusBoundaryGrouping(List<ApprovalStatusReportModel> boundaryModels)
    {
        var boundaryModelGroups = new Dictionary<string, List<ApprovalStatusReportModel>>();

        var availableOffices = boundaryModels.SelectMany(x => x.StatusPerOffice.Keys).Distinct().ToList();
        var reportModelGroups = boundaryModels.GroupBy(x => x.ApprovalStatus);

        foreach (var group in reportModelGroups)
        {
            var approvalStatus = group.Key;

            foreach (var office in availableOffices)
            {
                var officeTotal = group.Sum(x => x.StatusPerOffice.GetValueOrDefault(office, 0));
                var statusPerOffice = new Dictionary<string, int>
                {
                    { office, officeTotal }
                };
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

    public Dictionary<string, List<RootCauseReportModel>> RootCauseBoundaryGrouping(List<RootCauseReportModel> boundaryModels)
    {
        var boundaryModelGroups = new Dictionary<string, List<RootCauseReportModel>>();

        var availableOffices = boundaryModels.SelectMany(x => x.RootCausePerOffice.Keys).Distinct().ToList();
        var reportModelGroups = boundaryModels.GroupBy(x => x.RootCause);

        foreach (var group in reportModelGroups)
        {
            var rootCause = group.Key;

            foreach (var office in availableOffices)
            {
                var officeTotal = group.Sum(x => x.RootCausePerOffice.GetValueOrDefault(office, 0));
                var statusPerOffice = new Dictionary<string, int>
                {
                    { office, officeTotal }
                };
                var reportModel = new RootCauseReportModel(rootCause, statusPerOffice);

                if (!boundaryModelGroups.ContainsKey(office))
                {
                    boundaryModelGroups[office] = new List<RootCauseReportModel>();
                }

                boundaryModelGroups[office].Add(reportModel);
            }
        }

        return boundaryModelGroups;
    }

    public List<RootCauseReportModel> SortRootCauseModels(List<RootCauseReportModel> boundaryModels)
    {
        var orderedModels = boundaryModels.OrderByDescending(m => m.GrandTotal).ToList();

        var listA = orderedModels.Take(9).ToList();
        var listB = orderedModels.Skip(9).ToList();
        // var listA = orderedModels.Where(m => m.GrandTotal > 0).Take(9).ToList();
        // var listB = orderedModels.Where(m => m.GrandTotal == 0).Concat(orderedModels.Where(m => m.GrandTotal > 0).Skip(9)).ToList();

        var etcRootCause = "Lain-lain";
        var etcModel = new RootCauseReportModel(etcRootCause, new Dictionary<string, int>());
        var officeKeys = listB.SelectMany(m => m.RootCausePerOffice.Keys).Distinct().ToList();

        foreach (var officeKey in officeKeys)
        {
            etcModel.RootCausePerOffice[officeKey] = listB.Sum(m => m.RootCausePerOffice.GetValueOrDefault(officeKey, 0));
        }

        var result = new List<RootCauseReportModel>(listA)
        {
            etcModel
        };

        return result;
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
