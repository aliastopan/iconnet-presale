using ClosedXML.Excel;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public class WorksheetService
{
    protected readonly int _processorCount;
    protected readonly ParallelOptions _parallelOptions;

    public WorksheetService()
    {
        _processorCount = Environment.ProcessorCount;
        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _processorCount
        };
    }

    public byte[] GenerateXlsxBytes(IQueryable<WorkPaper>? presaleData)
    {
        using var workbook = new XLWorkbook();
        using var memoryStream = new MemoryStream();

        var exportModels = ConvertToExportModels(presaleData);
        var worksheet = workbook.Worksheets.Add("Presale Data");

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

        int row = 2;

        foreach (var exportModel in exportModels)
        {
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

            worksheet.Cell(row, 19).Value = exportModel.Bagian;
            worksheet.Cell(row, 20).Value = exportModel.KantorPerwakilan;
            worksheet.Cell(row, 21).Value = exportModel.Provinsi;
            worksheet.Cell(row, 22).Value = exportModel.Kabupaten;
            worksheet.Cell(row, 23).Value = exportModel.Kecamatan;
            worksheet.Cell(row, 24).Value = exportModel.Kelurahan;
            worksheet.Cell(row, 25).Value = $"{exportModel.KoordinatLatitude}, {exportModel.KoordinatLongitude}";

            row++;
        }

        workbook.SaveAs(memoryStream);

        return memoryStream.ToArray();
    }

    public List<WorkPaperExportModel> ConvertToExportModels(IQueryable<WorkPaper>? presaleData)
    {
        if (presaleData == null)
        {
            return new List<WorkPaperExportModel>();
        }

        var workPapers = presaleData.ToList();
        var exportModels = new List<WorkPaperExportModel>();
        var batchSize = 100;

        for (int i = 0; i < workPapers.Count; i += batchSize)
        {
            var batch = workPapers.Skip(i).Take(batchSize).ToList();
            Parallel.ForEach(batch, _parallelOptions, workPaper =>
            {
                var model = new WorkPaperExportModel(workPaper);
                lock (exportModels)
                {
                    exportModels.Add(model);
                }
            });
        }

        return exportModels;
    }
}
