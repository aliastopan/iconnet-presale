namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class InProgressReportModel
{
    public InProgressReportModel(WorkPaperLevel workPaperLevel,
        List<string> offices, List<int> statusPerOffice)
    {
        WorkPaperLevel = workPaperLevel;
        StatusPerOffice = new Dictionary<string, int>();

        // ensure number of offices and status per office are equal
        if (offices.Count != statusPerOffice.Count)
        {
            throw new ArgumentException();
        }

        // populate dictionary
        for (int i = 0; i < offices.Count; i++)
        {
            StatusPerOffice[offices[i]] = statusPerOffice[i];
        }
    }

    public InProgressReportModel(WorkPaperLevel workPaperLevel, Dictionary<string, int> statusPerOffice)
    {
        WorkPaperLevel = workPaperLevel;
        StatusPerOffice = statusPerOffice;
    }

    public WorkPaperLevel WorkPaperLevel { get; init; }
    public string InProgressDisplay => GetInProgressDisplay(WorkPaperLevel);
    public Dictionary<string, int> StatusPerOffice { get; init; } = default!;
    public int GrandTotal => GetGrandTotal();

    private int GetGrandTotal()
    {
        int grandTotal = 0;

        foreach (var value in StatusPerOffice.Values)
        {
            grandTotal += value;
        }

        return grandTotal;
    }

    private string GetInProgressDisplay(WorkPaperLevel workPaperLevel)
    {
        switch (workPaperLevel)
        {
            case WorkPaperLevel.ImportUnverified:
                return "Verifikasi";
            case WorkPaperLevel.Reinstated:
                return "Verifikasi (Reset)";
            case WorkPaperLevel.ImportVerified:
                return "Chat/Call Pick-Up";
            case WorkPaperLevel.Validating:
                return "Validasi";
            case WorkPaperLevel.WaitingApproval:
                return "Approval";
            default:
                throw new NotImplementedException("Invalid In-Progress Report Target");
        }
    }
}
