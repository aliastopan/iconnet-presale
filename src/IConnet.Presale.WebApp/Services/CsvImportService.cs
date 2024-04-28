namespace IConnet.Presale.WebApp.Services;

public class CsvImportService
{
    public bool TryGetCsvFromLocal(FileInfo localFile, out List<string[]>? csv)
    {
        csv = null;

        using var reader = localFile.OpenText();

        string[]? firstLine = reader.ReadLine()?.Split(',');
        string? line;

        if (firstLine == null)
        {
            Log.Warning("File is empty.");
            return false;
        }

        int totalColumn = firstLine.Length;
        var csvData = new List<string[]>
        {
            firstLine
        };

        while ((line = reader.ReadLine()) != null)
        {
            var values = line.Split(',');
            if (values.Length == totalColumn)
            {
                csvData.Add(values);
            }
            else
            {
                // If we encounter a line with an incorrect number of values,
                // we return false immediately without processing any more lines.
                csv = null;

                return false;
            }
        }

        csv = csvData;

        return true;
    }
}
