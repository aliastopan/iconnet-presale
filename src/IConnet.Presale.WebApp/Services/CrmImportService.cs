using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public sealed class CrmImportService
{
    public CrmImportService()
    {
        ApprovalOpportunities = new List<ImportModel>();
    }

    public List<ImportModel> ApprovalOpportunities { get; set; }

    public string[] SplitBySpecialCharacters(string input)
    {
        char[] delimiters = new char[] { '\t', '\n' };
        return input.Split(delimiters);
    }

    public int EmptySplitCount(string[] strings)
    {
        int counter = 0;
        foreach (var col in strings)
        {
            if (string.IsNullOrWhiteSpace(col))
            {
                counter++;
            }
        }

        return counter;
    }

    public void CountSpecialCharacters(string input, out int tabCount, out int newlineCount)
    {
        tabCount = 0;
        newlineCount = 0;
        foreach (char c in input)
        {
            if (c == '\t')
            {
                tabCount++;
            }
            else if (c == '\n')
            {
                newlineCount++;
            }
        }
    }
}
