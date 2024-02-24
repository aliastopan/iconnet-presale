using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Factories;

internal sealed class WorkloadFactory
{
    private readonly IDateTimeService _dateTimeService;

    public WorkloadFactory(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    internal ApprovalOpportunity CreateApprovalOpportunity(IApprovalOpportunityModel importModel)
    {
        return new ApprovalOpportunity
        {
            IdPermohonan = importModel.IdPermohonan,
            TglPermohonan = _dateTimeService.ParseExact(importModel.TglPermohonan),
            SumberPermohonan = importModel.SumberPermohonan,
            StatusPermohonan = importModel.StatusPermohonan,
            JenisPermohonan = importModel.JenisPermohonan,
            Layanan = importModel.Layanan,
            Splitter = importModel.Splitter,
            Pemohon = new Applicant
            {
                NamaLengkap = importModel.NamaPemohon,
                IdPln = importModel.IdPln,
                Email = importModel.EmailPemohon,
                NomorTelepon = importModel.TeleponPemohon,
                Nik = importModel.NikPemohon,
                Npwp = importModel.NpwpPemohon,
                Keterangan = importModel.Keterangan,
                Alamat = importModel.AlamatPemohon
            },
            Agen = new Salesperson
            {
                NamaLengkap = importModel.NamaAgen,
                Email = importModel.EmailAgen,
                NomorTelepon = importModel.TeleponAgen,
                Mitra = importModel.MitraAgen
            },
            Regional = new Regional
            {
                Bagian = importModel.Regional,
                KantorPerwakilan = importModel.KantorPerwakilan,
                Provinsi = importModel.Provinsi,
                Kabupaten = importModel.Kabupaten,
                Kecamatan = importModel.Kecamatan,
                Kelurahan = importModel.Kelurahan,
                Koordinat = new Coordinate
                {
                    Latitude = importModel.Latitude,
                    Longitude = importModel.Longitude
                }
            },
            StatusImport = ImportStatus.Unverified,
            ImportSignature = new ActionSignature
            {
                AccountIdSignature = importModel.ImportAccountIdSignature,
                Alias = importModel.ImportAliasSignature,
                TglAksi = importModel.TglImport,

            },
            ImportVerifikasiSignature = new ActionSignature
            {
                AccountIdSignature = Guid.Empty,
                Alias = string.Empty,
                TglAksi = _dateTimeService.Zero
            }
        };
    }

    internal WorkPaper CreateWorkPaper(IApprovalOpportunityModel importModel)
    {
        var approvalOpportunity = CreateApprovalOpportunity(importModel);

        return new WorkPaper
        {
            FkApprovalOpportunityId = approvalOpportunity.ApprovalOpportunityId,
            ApprovalOpportunity = approvalOpportunity,
            Shift = string.Empty,
            PersonInCharge = new PersonInCharge
            {
                Helpdesk = string.Empty,
                PlanningAssetCoverage = string.Empty
            },
            HelpdeskInCharge = new ActionSignature
            {
                AccountIdSignature = Guid.Empty,
                Alias = string.Empty,
                TglAksi = _dateTimeService.Zero
            },
            PlanningAssetCoverageInCharge = new ActionSignature
            {
                AccountIdSignature = Guid.Empty,
                Alias = string.Empty,
                TglAksi = _dateTimeService.Zero
            },
            ProsesValidasi = new ValidationProcess
            {
                ChatCallMulai = new ActionSignature
                {
                    AccountIdSignature = Guid.Empty,
                    Alias = string.Empty,
                    TglAksi = _dateTimeService.Zero
                },
                ChatCallRespons = new ActionSignature
                {
                    AccountIdSignature = Guid.Empty,
                    Alias = string.Empty,
                    TglAksi = _dateTimeService.Zero
                },
                WaktuTanggalRespons = _dateTimeService.Zero,
                LinkRekapChatHistory = string.Empty,
                ParameterValidasi = new ValidationParameter
                {
                    ValidasiIdPln = ValidationStatus.MenungguValidasi,
                    ValidasiNama = ValidationStatus.MenungguValidasi,
                    ValidasiNomorTelepon = ValidationStatus.MenungguValidasi,
                    ValidasiEmail = ValidationStatus.MenungguValidasi,
                    ValidasiAlamat = ValidationStatus.MenungguValidasi,
                    ShareLoc = new Coordinate
                    {
                        Latitude = string.Empty,
                        Longitude = string.Empty
                    }
                },
                Keterangan = string.Empty
            },
            ProsesApproval = new ApprovalProcess
            {
                StatusApproval = string.Empty,
                RootCause = string.Empty,
                Keterangan = string.Empty,
                JarakShareLoc = string.Empty,
                JarakICrmPlus = string.Empty,
                VaTerbit = _dateTimeService.Zero
            }
        };
    }
}
