using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class WorkloadManager : IWorkloadManager
{
    private readonly IDateTimeService _dateTimeService;

    public WorkloadManager(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public Task CreateWorkPaper(IApprovalOpportunityModel ImportModel)
    {
        var ApprovalOpportunity = new ApprovalOpportunity
        {
            IdPermohonan = ImportModel.IdPermohonan,
            TglPermohonan = _dateTimeService.ParseExact(ImportModel.TglPermohonan),
            SumberPermohonan = ImportModel.SumberPermohonan,
            StatusPermohonan = ImportModel.StatusPermohonan,
            JenisPermohonan = ImportModel.JenisPermohonan,
            Layanan = ImportModel.Layanan,
            Splitter = ImportModel.Splitter,
            Pemohon = new Applicant
            {
                NamaLengkap = ImportModel.NamaPemohon,
                IdPln = ImportModel.IdPln,
                Email = ImportModel.EmailPemohon,
                NomorTelepon = ImportModel.TeleponPemohon,
                Nik = ImportModel.NikPemohon,
                Npwp = ImportModel.NpwpPemohon,
                Keterangan = ImportModel.Keterangan,
                Alamat = ImportModel.AlamatPemohon
            },
            Agen = new Agent
            {
                NamaLengkap = ImportModel.NamaAgen,
                Email = ImportModel.EmailAgen,
                NomorTelepon = ImportModel.TeleponAgen,
                Mitra = ImportModel.MitraAgen
            },
            Regional = new Regional
            {
                Bagian = ImportModel.Regional,
                KantorPerwakilan = ImportModel.KantorPerwakilan,
                Provinsi = ImportModel.Provinsi,
                Kabupaten = ImportModel.Kabupaten,
                Kecamatan = ImportModel.Kecamatan,
                Kelurahan = ImportModel.Kelurahan,
                Koordinat = new Coordinate
                {
                    Latitude = ImportModel.Latitude,
                    Longitude = ImportModel.Longitude
                }
            },
            TglImport = ImportModel.TglImport.DateTime,
            NamaClaimImport = ImportModel.NamaClaimImport
        };

       return Task.CompletedTask;
    }
}
