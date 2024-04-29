namespace IConnet.Presale.WebApp.Services;

public class CsvImportService
{
    public bool TryGetCsvFromLocal(FileInfo localFile, out List<string[]>? csv)
    {
        csv = null;

        using StreamReader reader = localFile.OpenText();

        string[]? columnHeader = reader.ReadLine()?.Split(';');
        string? line;

        if (columnHeader == null)
        {
            Log.Warning("File is empty.");
            return false;
        }

        int totalColumn = columnHeader.Length;
        Log.Information("Column Header: {0}", totalColumn);

        var csvData = new List<string[]>
        {
            columnHeader
        };

        if (!IsCsvHeaderValid(columnHeader, out (bool isMatch, string column)[] headerChecks))
        {
            Log.Warning("INVALID CRM CSV");

            foreach (var header in headerChecks)
            {
                Log.Warning("INVALID: {0}", header);
            }

            return false;
        }
        else
        {
            Log.Information("VALID CSV");
        }

        while ((line = reader.ReadLine()) != null)
        {
            var values = line.Split(';');
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

    public bool IsCsvHeaderValid(string[] header, out (bool isMatch, string column)[] headerChecks)
    {
        headerChecks =
        [
            ValidateHeader(header[0], "ID PERMOHONAN"),
            ValidateHeader(header[1], "TGL PERMOHONAN"),
            ValidateHeader(header[2], "DURASI TIDAK LANJUT"),
            ValidateHeader(header[3], "NAMA PEMOHON"),
            ValidateHeader(header[4], "ID PLN"),
            ValidateHeader(header[5], "LAYANAN"),
            ValidateHeader(header[6], "SUMBER PERMOHONAN"),
            ValidateHeader(header[7], "STATUS PERMOHONAN"),
            ValidateHeader(header[8], "NAMA AGEN"),
            ValidateHeader(header[9], "EMAIL AGEN"),
            ValidateHeader(header[10], "TELEPON AGEN"),
            ValidateHeader(header[11], "MITRA AGEN"),
            ValidateHeader(header[12], "SPLITTER"),
            ValidateHeader(header[13], "JENIS PERMOHONAN"),
            ValidateHeader(header[14], "TELEPON PEMOHON"),
            ValidateHeader(header[15], "EMAIL PEMOHON"),
            ValidateHeader(header[16], "NIK PEMOHON"),
            ValidateHeader(header[17], "NPWP PEMOHON"),
            ValidateHeader(header[18], "KETERANGAN"),
            ValidateHeader(header[19], "ALAMAT"),
            ValidateHeader(header[20], "REGIONAL"),
            ValidateHeader(header[21], "KANTOR PERWAKILAN"),
            ValidateHeader(header[22], "PROVINSI"),
            ValidateHeader(header[23], "KABUPATEN"),
            ValidateHeader(header[24], "KECAMATAN"),
            ValidateHeader(header[25], "KELURAHAN"),
            ValidateHeader(header[26], "LATITUDE"),
            ValidateHeader(header[27], "LONGITUDE"),
        ];

        headerChecks = headerChecks.Where(tuple => !tuple.isMatch).ToArray();

        return headerChecks.All(tuple => tuple.isMatch);
    }

    public (bool, string) ValidateHeader(string value, string expected)
    {
        var result = (string.Equals(value, expected, StringComparison.OrdinalIgnoreCase), expected);

        if (!result.Item1)
        {
            Log.Warning("INVALID: {0}/{1}", value, expected);
        }

        return result;
    }
}
