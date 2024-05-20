namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class RootCauseClassificationReportModel
{
    public RootCauseClassificationReportModel(string classification,
        List<string> offices, List<int> classificationPerOffice)
    {
        Classification = classification;
        ClassificationPerOffice = new Dictionary<string, int>();

        if (offices.Count != classificationPerOffice.Count)
        {
            throw new ArgumentException();
        }

        for (int i = 0; i < offices.Count; i++)
        {
            ClassificationPerOffice[offices[i]] = classificationPerOffice[i];
        }
    }

    public RootCauseClassificationReportModel(string classification, Dictionary<string, int> classificationPerOffice)
    {
        Classification = classification;
        ClassificationPerOffice = classificationPerOffice;
    }

    public string Classification { get; init; }
    public Dictionary<string, int> ClassificationPerOffice { get; init; } = default!;
    public int GrandTotal => GetGrandTotal();

    private int GetGrandTotal()
    {
        int grandTotal = 0;

        foreach (var value in ClassificationPerOffice.Values)
        {
            grandTotal += value;
        }

        return grandTotal;
    }
}
