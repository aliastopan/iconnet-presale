namespace IConnet.Presale.WebApp.Helpers;

public class CrmImportMetadata
{
    public int StringLength { get; set; }
    public int NumberOfWhiteSpaces { get; set; }
    public int NumberOfTabSeparators { get; set; }
    public int NumberOfRows { get; set; }
    public int NumberOfDuplicates { get; set; }
    public bool IsValidImport { get; set; }
}
