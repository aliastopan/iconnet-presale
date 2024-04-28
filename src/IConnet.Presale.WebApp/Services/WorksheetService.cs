using ClosedXML.Excel;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public class WorksheetService
{
    private readonly AppSettingsService _appSettingsService;
    private readonly IntervalCalculatorService _intervalCalculatorService;

    private readonly int _processorCount;
    private readonly ParallelOptions _parallelOptions;

    private static readonly string _dateTimeFormat = @"yyyy-MM-dd HH:mm";
    private static readonly string _dateOnlyFormat = @"yyyy-MM-dd";
    private static readonly string _timeSpanFormat = @"[hh]:mm:ss";

    public WorksheetService(AppSettingsService appSettingsService,
        IntervalCalculatorService intervalCalculatorService)
    {
        _appSettingsService = appSettingsService;
        _intervalCalculatorService = intervalCalculatorService;

        _processorCount = Environment.ProcessorCount;
        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _processorCount
        };
    }

    public byte[] GenerateStandardXlsxBytes(IQueryable<WorkPaper>? presaleData, string worksheetName = "Presale Data")
    {
        using var workbook = new XLWorkbook();
        using var memoryStream = new MemoryStream();

        var exportModels = ConvertToPresaleDataExportModels(presaleData);
        var worksheet = workbook.Worksheets.Add(worksheetName);

        SetStandardWorksheet(worksheet);
        PopulateStandardWorksheet(exportModels, worksheet);

        workbook.SaveAs(memoryStream);

        return memoryStream.ToArray();
    }

    public byte[] GenerateAgingImportXlsxBytes(IQueryable<WorkPaper>? presaleData)
    {
        using var workbook = new XLWorkbook();
        using var memoryStream = new MemoryStream();

        var agingModels = ConvertToPresaleAgingModels(presaleData);
        var worksheet = workbook.Worksheets.Add("Aging Import");

        SetAgingImportWorksheet(worksheet);
        PopulateAgingImportWorksheet(agingModels, worksheet);

        workbook.SaveAs(memoryStream);

        return memoryStream.ToArray();
    }

    public byte[] GenerateAgingVerificationXlsxBytes(IQueryable<WorkPaper>? presaleData)
    {
        using var workbook = new XLWorkbook();
        using var memoryStream = new MemoryStream();

        var agingModels = ConvertToPresaleAgingModels(presaleData);
        var worksheet = workbook.Worksheets.Add("Aging Verification");

        SetAgingVerificationWorksheet(worksheet);
        PopulateAgingVerificationWorksheet(agingModels, worksheet);

        workbook.SaveAs(memoryStream);

        return memoryStream.ToArray();
    }

    public byte[] GenerateAgingChatCallMulaiXlsxBytes(IQueryable<WorkPaper>? presaleData)
    {
        using var workbook = new XLWorkbook();
        using var memoryStream = new MemoryStream();

        var agingModels = ConvertToPresaleAgingModels(presaleData);
        var worksheet = workbook.Worksheets.Add("Aging Pick-Up");

        SetAgingChatCallMulaiWorksheet(worksheet);
        PopulateAgingChatCallMulaiWorksheet(agingModels, worksheet);

        workbook.SaveAs(memoryStream);

        return memoryStream.ToArray();
    }

    public byte[] GenerateAgingChatCallResponsXlsxBytes(IQueryable<WorkPaper>? presaleData)
    {
        using var workbook = new XLWorkbook();
        using var memoryStream = new MemoryStream();

        var agingModels = ConvertToPresaleAgingModels(presaleData);
        var worksheet = workbook.Worksheets.Add("Aging Validasi");

        SetAgingChatCallResponsWorksheet(worksheet);
        PopulateAgingChatCallResponsWorksheet(agingModels, worksheet);

        workbook.SaveAs(memoryStream);

        return memoryStream.ToArray();
    }


    private List<PresaleDataXlsxModel> ConvertToPresaleDataExportModels(IQueryable<WorkPaper>? presaleData)
    {
        if (presaleData == null)
        {
            return new List<PresaleDataXlsxModel>();
        }

        var workPapers = presaleData.ToList();
        var exportModels = new List<PresaleDataXlsxModel>();
        var batchSize = 100;

        for (int i = 0; i < workPapers.Count; i += batchSize)
        {
            var batch = workPapers.Skip(i).Take(batchSize).ToList();
            Parallel.ForEach(batch, _parallelOptions, workPaper =>
            {
                var model = new PresaleDataXlsxModel(workPaper);
                lock (exportModels)
                {
                    exportModels.Add(model);
                }
            });
        }

        return exportModels.OrderBy(x => x.TglPermohonan).ToList();
    }

    private List<PresaleAgingXlsxModel> ConvertToPresaleAgingModels(IQueryable<WorkPaper>? presaleData)
    {
        if (presaleData == null)
        {
            return new List<PresaleAgingXlsxModel>();
        }

        var workPapers = presaleData.ToList();
        var exportModels = new List<PresaleAgingXlsxModel>();
        var batchSize = 100;

        for (int i = 0; i < workPapers.Count; i += batchSize)
        {
            var batch = workPapers.Skip(i).Take(batchSize).ToList();
            Parallel.ForEach(batch, _parallelOptions, workPaper =>
            {
                var model = new PresaleAgingXlsxModel(workPaper, _intervalCalculatorService);
                lock (exportModels)
                {
                    exportModels.Add(model);
                }
            });
        }

        return exportModels.OrderBy(x => x.TglPermohonan).ToList();
    }

    private static void SetStandardWorksheet(IXLWorksheet worksheet)
    {
        // approval opportunity
        worksheet.Cell(1, 1).Value = "ID PERMOHONAN";
        worksheet.Cell(1, 2).Value = "TGL PERMOHONAN";
        worksheet.Cell(1, 3).Value = "STATUS PERMOHONAN";
        worksheet.Cell(1, 4).Value = "LAYANAN";
        worksheet.Cell(1, 5).Value = "SUMBER PERMOHONAN";
        worksheet.Cell(1, 6).Value = "SPLITTER";

        worksheet.Cell(1, 7).Value = "ID PLN";
        worksheet.Cell(1, 8).Value = "NAMA PEMOHON";
        worksheet.Cell(1, 9).Value = "NO. TELP PEMOHON";
        worksheet.Cell(1, 10).Value = "EMAIL PEMOHON";
        worksheet.Cell(1, 11).Value = "ALAMAT PEMOHON";
        worksheet.Cell(1, 12).Value = "NIK PEMOHON";
        worksheet.Cell(1, 13).Value = "NPWP PEMOHON";
        worksheet.Cell(1, 14).Value = "KETERANGAN PEMOHON";

        worksheet.Cell(1, 15).Value = "NAMA AGEN";
        worksheet.Cell(1, 16).Value = "EMAIL AGEN";
        worksheet.Cell(1, 17).Value = "NO. TELP AGEN";
        worksheet.Cell(1, 18).Value = "MITRA AGEN";

        worksheet.Cell(1, 19).Value = "REGIONAL";
        worksheet.Cell(1, 20).Value = "KANTOR PERWAKILAN";
        worksheet.Cell(1, 21).Value = "PROVINSI";
        worksheet.Cell(1, 22).Value = "KABUPATEN";
        worksheet.Cell(1, 23).Value = "KECAMATAN";
        worksheet.Cell(1, 24).Value = "KELURAHAN";
        worksheet.Cell(1, 25).Value = "KOORDINAT";

        // workpaper
        worksheet.Cell(1, 26).Value = "SHIFT";
        worksheet.Cell(1, 27).Value = "WAKTU/TGL RESPONS";
        worksheet.Cell(1, 28).Value = "LINK CHAT HISTORY";
        worksheet.Cell(1, 29).Value = "VALIDASI ID PLN";
        worksheet.Cell(1, 30).Value = "VALIDASI NAMA";
        worksheet.Cell(1, 31).Value = "VALIDASI NO. TELP";
        worksheet.Cell(1, 32).Value = "VALIDASI EMAIL";
        worksheet.Cell(1, 33).Value = "VALIDASI ALAMAT";
        worksheet.Cell(1, 34).Value = "SHARE LOC";
        worksheet.Cell(1, 35).Value = "KETERANGAN VALIDASI";

        worksheet.Cell(1, 36).Value = "APPROVAL STATUS";
        worksheet.Cell(1, 37).Value = "DIRECT APPROVAL";
        worksheet.Cell(1, 38).Value = "ROOT CAUSE";
        worksheet.Cell(1, 39).Value = "KETERANGAN APPROVAL";
        worksheet.Cell(1, 40).Value = "JARAK SHARE LOC";
        worksheet.Cell(1, 41).Value = "JARAK iCRM+";
        worksheet.Cell(1, 42).Value = "SPLITTER GANTI";
        worksheet.Cell(1, 43).Value = "VA TERBIT";

        for (int i = 1; i <= 44; i++)
        {
            worksheet.Column(i).Width = 20;
        }

        worksheet.Column("B").Style.DateFormat.Format = _dateTimeFormat;     // tgl permohonan
        worksheet.Column("AA").Style.DateFormat.Format = _dateTimeFormat;    // waktu/tgl respons
        worksheet.Column("AQ").Style.DateFormat.Format = _dateOnlyFormat;    // va terbit
    }

    private void PopulateStandardWorksheet(List<PresaleDataXlsxModel> exportModels, IXLWorksheet worksheet)
    {
        int batchSize = 100;
        int numberOfBatches = (exportModels.Count + batchSize - 1) / batchSize;

        Parallel.For(0, numberOfBatches, _parallelOptions, batchIndex =>
        {
            int startIndex = batchIndex * batchSize;
            int endIndex = Math.Min(startIndex + batchSize, exportModels.Count);

            lock (worksheet)
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    var exportModel = exportModels[i];
                    int row = i + 2;

                    worksheet.Cell(row, 1).Value = exportModel.IdPermohonan;
                    worksheet.Cell(row, 2).Value = exportModel.TglPermohonan;
                    worksheet.Cell(row, 3).Value = exportModel.StatusPermohonan;
                    worksheet.Cell(row, 4).Value = exportModel.Layanan;
                    worksheet.Cell(row, 5).Value = exportModel.SumberPermohonan;
                    worksheet.Cell(row, 6).Value = exportModel.Splitter;

                    worksheet.Cell(row, 7).Value = exportModel.IdPlnPelanggan;
                    worksheet.Cell(row, 8).Value = exportModel.NamaPelanggan;
                    worksheet.Cell(row, 9).Value = exportModel.NomorTeleponPelanggan;
                    worksheet.Cell(row, 10).Value = exportModel.EmailPelanggan;
                    worksheet.Cell(row, 11).Value = exportModel.AlamatPelanggan;
                    worksheet.Cell(row, 12).Value = exportModel.NikPelanggan;
                    worksheet.Cell(row, 13).Value = exportModel.NpwpPelanggan;
                    worksheet.Cell(row, 14).Value = exportModel.KeteranganPelanggan;

                    worksheet.Cell(row, 15).Value = exportModel.NamaAgen;
                    worksheet.Cell(row, 16).Value = exportModel.EmailAgen;
                    worksheet.Cell(row, 17).Value = exportModel.NomorTeleponAgen;
                    worksheet.Cell(row, 18).Value = exportModel.MitraAgen;

                    worksheet.Cell(row, 19).Value = exportModel.RegionalBagian;
                    worksheet.Cell(row, 20).Value = exportModel.KantorPerwakilan;
                    worksheet.Cell(row, 21).Value = exportModel.Provinsi;
                    worksheet.Cell(row, 22).Value = exportModel.Kabupaten;
                    worksheet.Cell(row, 23).Value = exportModel.Kecamatan;
                    worksheet.Cell(row, 24).Value = exportModel.Kelurahan;
                    worksheet.Cell(row, 25).Value = exportModel.Koordinat;

                    worksheet.Cell(row, 26).Value = exportModel.Shift;
                    worksheet.Cell(row, 27).Value = exportModel.GetWaktuTanggalRespons();
                    worksheet.Cell(row, 28).Value = exportModel.LinkChatHistory;
                    worksheet.Cell(row, 29).Value = exportModel.ValidasiIdPln;
                    worksheet.Cell(row, 30).Value = exportModel.ValidasiNama;
                    worksheet.Cell(row, 31).Value = exportModel.ValidasiNomorTelepon;
                    worksheet.Cell(row, 32).Value = exportModel.ValidasiEmail;
                    worksheet.Cell(row, 33).Value = exportModel.ValidasiAlamat;
                    worksheet.Cell(row, 34).Value = exportModel.ShareLoc;
                    worksheet.Cell(row, 35).Value = exportModel.KeteranganValidasi;

                    worksheet.Cell(row, 36).Value = exportModel.StatusApproval;
                    worksheet.Cell(row, 37).Value = exportModel.GetDirectApproval();
                    worksheet.Cell(row, 38).Value = exportModel.RootCause;
                    worksheet.Cell(row, 39).Value = exportModel.KeteranganApproval;
                    worksheet.Cell(row, 40).Value = exportModel.JarakShareLoc;
                    worksheet.Cell(row, 41).Value = exportModel.JarakICrmPlus;
                    worksheet.Cell(row, 42).Value = exportModel.SplitterGanti;
                    worksheet.Cell(row, 43).Value = exportModel.GetVaTerbit();
                }
            }
        });
    }

    private static void SetAgingImportWorksheet(IXLWorksheet worksheet)
    {
        worksheet.Cell(1, 1).Value = "ID PERMOHONAN";
        worksheet.Cell(1, 2).Value = "TGL PERMOHONAN";
        worksheet.Cell(1, 3).Value = "PIC IMPORT";
        worksheet.Cell(1, 4).Value = "TGL/WAKTU IMPORT";
        worksheet.Cell(1, 5).Value = "AGING IMPORT";
        worksheet.Cell(1, 6).Value = "SLA";

        for (int i = 1; i <= 6; i++)
        {
            worksheet.Column(i).Width = 20;
        }

        worksheet.Column("B").Style.DateFormat.Format = _dateTimeFormat;     // tgl permohonan
        worksheet.Column("D").Style.DateFormat.Format = _dateTimeFormat;     // waktu/tgl import
        worksheet.Column("E").Style.NumberFormat.Format = _timeSpanFormat;   // aging import
    }

    private static void SetAgingVerificationWorksheet(IXLWorksheet worksheet)
    {
        worksheet.Cell(1, 1).Value = "ID PERMOHONAN";
        worksheet.Cell(1, 2).Value = "TGL PERMOHONAN";
        worksheet.Cell(1, 3).Value = "PIC VERIFICATION";
        worksheet.Cell(1, 4).Value = "TGL/WAKTU VERIFICATION";
        worksheet.Cell(1, 5).Value = "AGING VERIFICATION";

        for (int i = 1; i <= 5; i++)
        {
            worksheet.Column(i).Width = 20;
        }

        worksheet.Column("B").Style.DateFormat.Format = _dateTimeFormat;     // tgl permohonan
        worksheet.Column("D").Style.DateFormat.Format = _dateTimeFormat;     // waktu/tgl verifikasi
        worksheet.Column("E").Style.NumberFormat.Format = _timeSpanFormat;   // aging verifikasi
    }

    private static void SetAgingChatCallMulaiWorksheet(IXLWorksheet worksheet)
    {
        worksheet.Cell(1, 1).Value = "ID PERMOHONAN";
        worksheet.Cell(1, 2).Value = "TGL PERMOHONAN";
        worksheet.Cell(1, 3).Value = "PIC CHAT/CALL PICK-UP";
        worksheet.Cell(1, 4).Value = "TGL/WAKTU CHAT/CALL PICK-UP";
        worksheet.Cell(1, 5).Value = "AGING CHAT/CALL PICK-UP";
        worksheet.Cell(1, 6).Value = "SLA";

        for (int i = 1; i <= 6; i++)
        {
            worksheet.Column(i).Width = 20;
        }

        worksheet.Column("B").Style.DateFormat.Format = _dateTimeFormat;     // tgl permohonan
        worksheet.Column("D").Style.DateFormat.Format = _dateTimeFormat;     // waktu/tgl chat/call pick-up
        worksheet.Column("E").Style.NumberFormat.Format = _timeSpanFormat;   // aging chat/call pick-up
    }

    private static void SetAgingChatCallResponsWorksheet(IXLWorksheet worksheet)
    {
        worksheet.Cell(1, 1).Value = "ID PERMOHONAN";
        worksheet.Cell(1, 2).Value = "TGL PERMOHONAN";
        worksheet.Cell(1, 3).Value = "PIC CHAT/CALL VALIDASI";
        worksheet.Cell(1, 4).Value = "TGL/WAKTU CHAT/CALL VALIDASI";
        worksheet.Cell(1, 5).Value = "AGING CHAT/CALL VALIDASI";
        worksheet.Cell(1, 6).Value = "SLA";

        for (int i = 1; i <= 6; i++)
        {
            worksheet.Column(i).Width = 20;
        }

        worksheet.Column("B").Style.DateFormat.Format = _dateTimeFormat;     // tgl permohonan
        worksheet.Column("D").Style.DateFormat.Format = _dateTimeFormat;     // waktu/tgl chat/call validasi
        worksheet.Column("E").Style.NumberFormat.Format = _timeSpanFormat;   // aging chat/call validasi
    }


    private void PopulateAgingImportWorksheet(List<PresaleAgingXlsxModel> agingModels, IXLWorksheet worksheet)
    {
        int batchSize = 100;
        int numberOfBatches = (agingModels.Count + batchSize - 1) / batchSize;

        Parallel.For(0, numberOfBatches, _parallelOptions, batchIndex =>
        {
            int startIndex = batchIndex * batchSize;
            int endIndex = Math.Min(startIndex + batchSize, agingModels.Count);

            lock (worksheet)
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    var exportModel = agingModels[i];
                    int row = i + 2;

                    worksheet.Cell(row, 1).Value = exportModel.IdPermohonan;
                    worksheet.Cell(row, 2).Value = exportModel.TglPermohonan;
                    worksheet.Cell(row, 3).Value = exportModel.PicImport;
                    worksheet.Cell(row, 4).Value = exportModel.TimestampImport;
                    worksheet.Cell(row, 5).Value = exportModel.AgingImport;

                    string slaVerdict = exportModel.AgingImport < _appSettingsService.SlaImport
                        ? "WON"
                        : "LOST";

                    worksheet.Cell(row, 6).Value = slaVerdict;
                }
            }
        });
    }

    private void PopulateAgingVerificationWorksheet(List<PresaleAgingXlsxModel> agingModels, IXLWorksheet worksheet)
    {
        int batchSize = 100;
        int numberOfBatches = (agingModels.Count + batchSize - 1) / batchSize;

        Parallel.For(0, numberOfBatches, _parallelOptions, batchIndex =>
        {
            int startIndex = batchIndex * batchSize;
            int endIndex = Math.Min(startIndex + batchSize, agingModels.Count);

            lock (worksheet)
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    var exportModel = agingModels[i];
                    int row = i + 2;

                    worksheet.Cell(row, 1).Value = exportModel.IdPermohonan;
                    worksheet.Cell(row, 2).Value = exportModel.TglPermohonan;
                    worksheet.Cell(row, 3).Value = exportModel.PicVerifikasi;
                    worksheet.Cell(row, 4).Value = exportModel.TimestampVerifikasi;
                    worksheet.Cell(row, 5).Value = exportModel.AgingVerifikasi;
                }
            }
        });
    }

    private void PopulateAgingChatCallMulaiWorksheet(List<PresaleAgingXlsxModel> agingModels, IXLWorksheet worksheet)
    {
        int batchSize = 100;
        int numberOfBatches = (agingModels.Count + batchSize - 1) / batchSize;

        Parallel.For(0, numberOfBatches, _parallelOptions, batchIndex =>
        {
            int startIndex = batchIndex * batchSize;
            int endIndex = Math.Min(startIndex + batchSize, agingModels.Count);

            lock (worksheet)
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    var exportModel = agingModels[i];
                    int row = i + 2;

                    worksheet.Cell(row, 1).Value = exportModel.IdPermohonan;
                    worksheet.Cell(row, 2).Value = exportModel.TglPermohonan;
                    worksheet.Cell(row, 3).Value = exportModel.PicChatCallMulai;
                    worksheet.Cell(row, 4).Value = exportModel.TimestampChatCallMulai;
                    worksheet.Cell(row, 5).Value = exportModel.AgingChatCallMulai;

                    string slaVerdict = exportModel.AgingChatCallMulai < _appSettingsService.SlaPickUp
                        ? "WON"
                        : "LOST";

                    worksheet.Cell(row, 6).Value = slaVerdict;
                }
            }
        });
    }

    private void PopulateAgingChatCallResponsWorksheet(List<PresaleAgingXlsxModel> agingModels, IXLWorksheet worksheet)
    {
        int batchSize = 100;
        int numberOfBatches = (agingModels.Count + batchSize - 1) / batchSize;

        Parallel.For(0, numberOfBatches, _parallelOptions, batchIndex =>
        {
            int startIndex = batchIndex * batchSize;
            int endIndex = Math.Min(startIndex + batchSize, agingModels.Count);

            lock (worksheet)
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    var exportModel = agingModels[i];
                    int row = i + 2;

                    worksheet.Cell(row, 1).Value = exportModel.IdPermohonan;
                    worksheet.Cell(row, 2).Value = exportModel.TglPermohonan;
                    worksheet.Cell(row, 3).Value = exportModel.PicChatCallRespons;
                    worksheet.Cell(row, 4).Value = exportModel.TimestampChatCallRespons;
                    worksheet.Cell(row, 5).Value = exportModel.AgingChatCallRespons;

                    string slaVerdict = exportModel.AgingChatCallRespons < _appSettingsService.SlaValidasi
                        ? "WON"
                        : "LOST";

                    worksheet.Cell(row, 6).Value = slaVerdict;
                }
            }
        });
    }
}
