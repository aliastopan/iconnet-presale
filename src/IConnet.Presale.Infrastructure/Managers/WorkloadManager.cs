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
            ApplicationId = ImportModel.IdPermohonan,
            ApplicationDate = _dateTimeService.ParseExact(ImportModel.TglPermohonan),
            ApplicationSource = ImportModel.SumberPermohonan,
            ApplicationStatus = ImportModel.StatusPermohonan,
            ApplicationType = ImportModel.JenisPermohonan,
            Service = ImportModel.Layanan,
            Splitter = ImportModel.Splitter,
            Applicant = new Applicant
            {
                FullName = ImportModel.NamaPemohon,
                PlnId = ImportModel.IdPln,
                EmailAddress = ImportModel.EmailPemohon,
                PhoneNumber = ImportModel.TeleponPemohon,
                Nik = ImportModel.NikPemohon,
                Npwp = ImportModel.NpwpPemohon,
                Information = ImportModel.Keterangan,
                Address = ImportModel.AlamatPemohon
            },
            Agent = new Agent
            {
                FullName = ImportModel.NamaAgen,
                EmailAddress = ImportModel.EmailAgen,
                PhoneNumber = ImportModel.TeleponAgen,
                Partner = ImportModel.MitraAgen
            },
            Regional = new Regional
            {
                Bagian = ImportModel.Regional,
                RepresentativeOffice = ImportModel.KantorPerwakilan,
                Provinsi = ImportModel.Provinsi,
                Kabupaten = ImportModel.Kabupaten,
                Kecamatan = ImportModel.Kecamatan,
                Kelurahan = ImportModel.Kelurahan,
                Coordinate = new Coordinate
                {
                    Latitude = ImportModel.Latitude,
                    Longitude = ImportModel.Longitude
                }
            },
            ImportDateTime = ImportModel.ImportDateTime.DateTime,
            ImportClaimName = ImportModel.ImportClaimName
        };

       return Task.CompletedTask;
    }
}
