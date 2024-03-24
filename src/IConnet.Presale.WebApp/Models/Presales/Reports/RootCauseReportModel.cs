namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class RootCauseReportModel
{
    public RootCauseReportModel(string rootCause,
        List<string> offices, List<int> rootCausePerOffice)
    {
        RootCause = rootCause;
        RootCausePerOffice = new Dictionary<string, int>();

        if (offices.Count != rootCausePerOffice.Count)
        {
            throw new ArgumentException();
        }

        for (int i = 0; i < offices.Count; i++)
        {
            RootCausePerOffice[offices[i]] = rootCausePerOffice[i];
        }
    }

    public string RootCause { get; init; }
    public Dictionary<string, int> RootCausePerOffice { get; init; } = default!;
    public int GrandTotal => GetGrandTotal();

    private int GetGrandTotal()
    {
        int grandTotal = 0;

        foreach (var value in RootCausePerOffice.Values)
        {
            grandTotal += value;
        }

        return grandTotal;
    }
}
