namespace IConnet.Presale.WebApp.Models.Presales;

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

    public ApprovalStatus ApprovalStatus { get; set; }
    public string ApprovalStatusDisplay => EnumProcessor.EnumToDisplayString(ApprovalStatus);
    public Dictionary<string, int> StatusPerOffice { get; set; } = default!;
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
