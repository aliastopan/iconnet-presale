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
            worksheet.Column(i).Width = 32;
        }

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

            worksheet.Cell(row, 19).Value = exportModel.RegionalBagian;
            worksheet.Cell(row, 20).Value = exportModel.KantorPerwakilan;
            worksheet.Cell(row, 21).Value = exportModel.Provinsi;
            worksheet.Cell(row, 22).Value = exportModel.Kabupaten;
            worksheet.Cell(row, 23).Value = exportModel.Kecamatan;
            worksheet.Cell(row, 24).Value = exportModel.Kelurahan;
            worksheet.Cell(row, 25).Value = $"{exportModel.KoordinatLatitude}, {exportModel.KoordinatLongitude}";

            worksheet.Cell(row, 26).Value = exportModel.Shift;
            worksheet.Cell(row, 27).Value = exportModel.WaktuTanggalRespons;
            worksheet.Cell(row, 28).Value = exportModel.LinkChatHistory;
            worksheet.Cell(row, 29).Value = exportModel.ValidasiIdPln;
            worksheet.Cell(row, 30).Value = exportModel.ValidasiNama;
            worksheet.Cell(row, 31).Value = exportModel.ValidasiNomorTelepon;
            worksheet.Cell(row, 32).Value = exportModel.ValidasiEmail;
            worksheet.Cell(row, 33).Value = exportModel.ValidasiAlamat;
            worksheet.Cell(row, 34).Value = $"{exportModel.ShareLocLatitude}, {exportModel.ShareLocLongitude}";
            worksheet.Cell(row, 35).Value = exportModel.KeteranganValidasi;

            row++;
        }

        workbook.SaveAs(memoryStream);

        return memoryStream.ToArray();
    }

    public List<PresaleDataXlsxModel> ConvertToExportModels(IQueryable<WorkPaper>? presaleData)
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

        return exportModels;
    }
}
