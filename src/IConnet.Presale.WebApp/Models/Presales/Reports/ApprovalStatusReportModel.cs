namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ApprovalStatusReportModel
{
    public ApprovalStatusReportModel(ApprovalStatus approvalStatus,
        List<string> offices, List<int> statusPerOffice)
    {
        ApprovalStatus = approvalStatus;
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

    public ApprovalStatusReportModel(ApprovalStatus approvalStatus, Dictionary<string, int> statusPerOffice)
    {
        ApprovalStatus = approvalStatus;
        StatusPerOffice = statusPerOffice;
    }

    public ApprovalStatus ApprovalStatus { get; init; }
    public string ApprovalStatusDisplay => EnumProcessor.EnumToDisplayString(ApprovalStatus);
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
}


// public class ApprovalStatusReportModel
// {
//     public ApprovalStatus ApprovalStatus { get; init; }
//     public Dictionary<string, int> StatusPerOffice { get; init; } = default!;
// }
