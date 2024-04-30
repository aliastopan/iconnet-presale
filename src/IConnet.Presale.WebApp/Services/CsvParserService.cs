namespace IConnet.Presale.WebApp.Services;

public class CsvParserService
{
    public bool TryGetCsvFromLocal(FileInfo localFile, out List<string[]>? csv, out string errorMessage)
    {
        csv = null;
        errorMessage = string.Empty;

        using StreamReader reader = localFile.OpenText();

        string[]? columnHeader = reader.ReadLine()?.Split(';');
        string? line;

        if (columnHeader == null)
        {
            errorMessage = "File .csv tidak memiliki Header";
            return false;
        }

        int totalColumn = columnHeader.Length;

        if (columnHeader.Length < 28)
        {
            errorMessage = "File .csv terdapat Header tidak valid";

            return false;
        }

        List<string[]> csvContent = [];

        if (!IsCsvHeaderValid(columnHeader, out (bool isMatch, string column)[] headerChecks))
        {
            var invalidHeader = headerChecks.FirstOrDefault().column;

            errorMessage = $"File .csv terdapat Header tidak valid: '{invalidHeader}'";

            return false;
        }

        while ((line = reader.ReadLine()) != null)
        {
            var row = line.Split(';');
            if (row.Length == totalColumn)
            {
                if (IsCsvRowValid(row))
                {
                    csvContent.Add(row);
                }
            }
            else
            {
                errorMessage = "Konten file .csv terdapat baris yang tidak valid";
                csv = null;

                return false;
            }
        }

        csv = csvContent;

        return true;
    }

    private static  bool IsCsvHeaderValid(string[] header, out (bool isMatch, string column)[] headerChecks)
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

    private static  bool IsCsvRowValid(string[] column)
    {
        bool[] columnChecks =
        [
            column[0].HasValue(),   // ID PERMOHONAN
            column[1].HasValue(),   // TGL PERMOHONAN
            column[3].HasValue(),   // NAMA PEMOHON
            column[4].HasValue(),   // ID PLN
            column[5].HasValue(),   // LAYANAN
            column[8].HasValue(),   // NAMA AGEN
            column[9].HasValue(),   // EMAIL AGEN
            column[10].HasValue(),  // MITRA AGEN
            column[12].HasValue(),  // SPLITTER
            column[14].HasValue(),  // TELEPON PEMOHON
            column[15].HasValue(),  // EMAIL PEMOHON
            column[19].HasValue(),  // ALAMAT
            column[20].HasValue(),  // REGIONAL
            column[21].HasValue(),  // KANTOR PERWAKILAN
            column[22].HasValue(),  // PROVINSI
            column[23].HasValue(),  // KABUPATEN
            column[24].HasValue(),  // KECAMATAN
            column[25].HasValue(),  // KELURAHAN
            column[26].HasValue(),  // LATITUDE
            column[27].HasValue(),  // LONGITUDE
        ];

        return columnChecks.All(row => row);

    }

    private static (bool, string) ValidateHeader(string value, string expected)
    {
        var result = (string.Equals(value, expected, StringComparison.OrdinalIgnoreCase), expected);

        if (!result.Item1)
        {
            Log.Warning("INVALID: {0}/{1}", value, expected);
        }

        return result;
    }
}
