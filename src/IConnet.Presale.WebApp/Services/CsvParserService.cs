using ClosedXML.Excel;
using System.Globalization;

namespace IConnet.Presale.WebApp.Services;

public class CsvParserService
{
    private const int NumberOfColumn = 27;

    private readonly IDateTimeService _dateTimeService;

    public CsvParserService(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

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

        if (columnHeader.Length < NumberOfColumn)
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

        NormalizeDateTimeString(csvContent);

        var tglPermohonanString = GetTglPermohonanString(csvContent);
        var isAllValid = ValidateTglPermohonanString(tglPermohonanString);

        if(!isAllValid)
        {
            errorMessage = "Konten file .csv terdapat format TGL PERMOHONAN yang tidak valid";
            csv = null;

            return false;
        }

        csv = csvContent;

        return true;
    }

    private static List<string> GetTglPermohonanString(List<string[]> csv)
    {
        List<string> result = [];

        foreach (string[] row in csv)
        {
            result.Add(row[1]);
        }

        return result;
    }

    private static void NormalizeDateTimeString(List<string[]> csv)
    {
        string inputFormat = "dd/MM/yyyy h:mm:ss";
        string outputFormat = "dd/MM/yyyy HH:mm:ss";

        foreach (string[] row in csv)
        {
            string dateTimeString = row[1];

            if (DateTime.TryParseExact(dateTimeString, inputFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime dateTime))
            {
                // convert the parsed DateTime to the desired 24-hour format with leading zero for the hour
                string normalizeFormat = dateTime.ToString(outputFormat);
                row[1] = normalizeFormat;
            }
        }
    }

    public bool ValidateTglPermohonanString(List<string> tglPermohonanString)
    {
        var parsingFormat = _dateTimeService.GetParsingFormat();

        foreach (string dateTimeString in tglPermohonanString)
        {
            if (!DateTime.TryParseExact(dateTimeString, parsingFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsCsvHeaderValid(string[] header, out (bool isMatch, string column)[] headerChecks)
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
            ValidateHeader(header[13], "TELEPON PEMOHON"),
            ValidateHeader(header[14], "EMAIL PEMOHON"),
            ValidateHeader(header[15], "NIK PEMOHON"),
            ValidateHeader(header[16], "NPWP PEMOHON"),
            ValidateHeader(header[17], "KETERANGAN"),
            ValidateHeader(header[18], "ALAMAT"),
            ValidateHeader(header[19], "REGIONAL"),
            ValidateHeader(header[20], "KANTOR PERWAKILAN"),
            ValidateHeader(header[21], "PROVINSI"),
            ValidateHeader(header[22], "KABUPATEN"),
            ValidateHeader(header[23], "KECAMATAN"),
            ValidateHeader(header[24], "KELURAHAN"),
            ValidateHeader(header[25], "LATITUDE"),
            ValidateHeader(header[26], "LONGITUDE"),
        ];

        headerChecks = headerChecks.Where(tuple => !tuple.isMatch).ToArray();

        return headerChecks.All(tuple => tuple.isMatch);
    }

    private static bool IsCsvRowValid(string[] column)
    {
        bool[] columnChecks =
        [
            column[0].HasValue(),   // ID PERMOHONAN
            column[1].HasValue(),   // TGL PERMOHONAN
            column[3].HasValue(),   // NAMA PEMOHON
            column[4].HasValue(),   // ID PLN
            column[5].HasValue(),   // LAYANAN
            column[8].HasValue(),   // NAMA AGEN
            column[12].HasValue(),  // SPLITTER
            column[13].HasValue(),  // TELEPON PEMOHON
            column[14].HasValue(),  // EMAIL PEMOHON
            column[18].HasValue(),  // ALAMAT
            column[19].HasValue(),  // REGIONAL
            column[20].HasValue(),  // KANTOR PERWAKILAN
            column[21].HasValue(),  // PROVINSI
            column[22].HasValue(),  // KABUPATEN
            column[23].HasValue(),  // KECAMATAN
            column[24].HasValue(),  // KELURAHAN
            column[25].HasValue(),  // LATITUDE
            column[26].HasValue(),  // LONGITUDE
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

    public byte[] GenerateXlsxImportTemplateBytes()
    {
        using var workbook = new XLWorkbook();
        using var memoryStream = new MemoryStream();

        var worksheet = workbook.Worksheets.Add("Import Template");

        GenerateXlsxHeaderTemplate(worksheet);
        GenerateXlsxContentTemplate(worksheet);

        workbook.SaveAs(memoryStream);

        return memoryStream.ToArray();
    }

    private static void GenerateXlsxHeaderTemplate(IXLWorksheet worksheet)
    {
        worksheet.Cell(1, 1).Value = "ID PERMOHONAN";
        worksheet.Cell(1, 2).Value = "TGL PERMOHONAN";
        worksheet.Cell(1, 3).Value = "DURASI TIDAK LANJUT";
        worksheet.Cell(1, 4).Value = "NAMA PEMOHON";
        worksheet.Cell(1, 5).Value = "ID PLN";
        worksheet.Cell(1, 6).Value = "LAYANAN";
        worksheet.Cell(1, 7).Value = "SUMBER PERMOHONAN";
        worksheet.Cell(1, 8).Value = "STATUS PERMOHONAN";
        worksheet.Cell(1, 9).Value = "NAMA AGEN";
        worksheet.Cell(1, 10).Value = "EMAIL AGEN";
        worksheet.Cell(1, 11).Value = "TELEPON AGEN";
        worksheet.Cell(1, 12).Value = "MITRA AGEN";
        worksheet.Cell(1, 13).Value = "SPLITTER";
        worksheet.Cell(1, 14).Value = "TELEPON PEMOHON";
        worksheet.Cell(1, 15).Value = "EMAIL PEMOHON";
        worksheet.Cell(1, 16).Value = "NIK PEMOHON";
        worksheet.Cell(1, 17).Value = "NPWP PEMOHON";
        worksheet.Cell(1, 18).Value = "KETERANGAN";
        worksheet.Cell(1, 19).Value = "ALAMAT";
        worksheet.Cell(1, 20).Value = "REGIONAL";
        worksheet.Cell(1, 21).Value = "KANTOR PERWAKILAN";
        worksheet.Cell(1, 22).Value = "PROVINSI";
        worksheet.Cell(1, 23).Value = "KABUPATEN";
        worksheet.Cell(1, 24).Value = "KECAMATAN";
        worksheet.Cell(1, 25).Value = "KELURAHAN";
        worksheet.Cell(1, 26).Value = "LATITUDE";
        worksheet.Cell(1, 27).Value = "LONGITUDE";

        for (int i = 1; i <= NumberOfColumn; i++)
        {
            worksheet.Column(i).Width = 20;
        }

        // string[] formats = new[] { "yyyy-MM-dd HH:mm", "dd/MM/yyyy HH:mm" };

        string dateTimeFormat = @"dd/MM/yyyy HH:mm";

        worksheet.Column("A").Style.NumberFormat.Format = "@";              // id permohonan
        worksheet.Column("B").Style.DateFormat.Format = dateTimeFormat;     // tgl permohonan
        worksheet.Column("E").Style.NumberFormat.Format = "@";              // id pln
        worksheet.Column("K").Style.NumberFormat.Format = "@";              // telepon agen
        worksheet.Column("N").Style.NumberFormat.Format = "@";              // telepon pemohon
        worksheet.Column("P").Style.NumberFormat.Format = "@";              // nik pemohon
        worksheet.Column("Q").Style.NumberFormat.Format = "@";              // npwp pemohon
        worksheet.Column("Z").Style.NumberFormat.Format = "@";              // latitude
        worksheet.Column("AA").Style.NumberFormat.Format = "@";             // longitude
    }

    private static void GenerateXlsxContentTemplate(IXLWorksheet worksheet)
    {
        worksheet.Cell(2, 1).Value = "B14102403150015";
        worksheet.Cell(2, 2).Value = DateTime.Now.AddHours(-1).ToString("dd/MM/yyyy HH:mm");
        worksheet.Cell(2, 3).Value = "1 Jam";
        worksheet.Cell(2, 4).Value = "ANDI WIJAYA - DATA SAMPLE";
        worksheet.Cell(2, 5).Value = "911630073603";
        worksheet.Cell(2, 6).Value = "ICONNET-1 GBPS";
        worksheet.Cell(2, 7).Value = "CRMMOBILE";
        worksheet.Cell(2, 8).Value = "NEGOTIATION";
        worksheet.Cell(2, 9).Value = "AWANG S. PUTRA";
        worksheet.Cell(2, 10).Value = "awang@gmail.com";
        worksheet.Cell(2, 11).Value = "6281234567890";
        worksheet.Cell(2, 12).Value = "MITRA INDIVIDU PENJUALAN";
        worksheet.Cell(2, 13).Value = "SPLT_LMGA10480_01";
        worksheet.Cell(2, 14).Value = "6281331359720";
        worksheet.Cell(2, 15).Value = "andy.wijaya@gmail.com";
        worksheet.Cell(2, 16).Value = "3500000000000001";
        worksheet.Cell(2, 17).Value = "";
        worksheet.Cell(2, 18).Value = "INI DATA SAMPLE";
        worksheet.Cell(2, 19).Value = "Jl. Ahmad Yani No.88, Ketintang, Kec. Gayungan, Surabaya, Jawa Timur 60231";
        worksheet.Cell(2, 20).Value = "JAWA BAGIAN TIMUR";
        worksheet.Cell(2, 21).Value = "SURABAYA";
        worksheet.Cell(2, 22).Value = "Jawa Timur";
        worksheet.Cell(2, 23).Value = "Surabaya";
        worksheet.Cell(2, 24).Value = "Gayungan";
        worksheet.Cell(2, 25).Value = "Ketintang";
        worksheet.Cell(2, 26).Value = "-7.3213675";
        worksheet.Cell(2, 27).Value = "112.7307526";
    }
}
