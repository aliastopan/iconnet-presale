using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Factories;

internal sealed class WorkPaperFactory
{
    private readonly IDateTimeService _dateTimeService;

    public WorkPaperFactory(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    internal ApprovalOpportunity CreateApprovalOpportunity(IApprovalOpportunityModel importModel)
    {
        return new ApprovalOpportunity
        {
            ApprovalOpportunityId = Guid.NewGuid(),
            IdPermohonan = importModel.IdPermohonan,
            TglPermohonan = _dateTimeService.ParseExact(importModel.TglPermohonan),
            SumberPermohonan = importModel.SumberPermohonan,
            StatusPermohonan = importModel.StatusPermohonan,
            JenisPermohonan = importModel.JenisPermohonan,
            Layanan = importModel.Layanan,
            Splitter = importModel.Splitter,
            Pemohon = new Applicant
            {
                NamaPelanggan = importModel.NamaPemohon,
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
            SignatureImport = new ActionSignature
            {
                AccountIdSignature = importModel.ImportAccountIdSignature,
                Alias = importModel.ImportAliasSignature,
                TglAksi = importModel.TglImport,

            },
            SignatureVerifikasiImport = new ActionSignature
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
            WorkPaperId = Guid.NewGuid(),
            WorkPaperLevel = WorkPaperLevel.ImportUnverified,
            FkApprovalOpportunityId = approvalOpportunity.ApprovalOpportunityId,
            ApprovalOpportunity = approvalOpportunity,
            Shift = string.Empty,
            SignatureHelpdeskInCharge = new ActionSignature
            {
                AccountIdSignature = Guid.Empty,
                Alias = string.Empty,
                TglAksi = _dateTimeService.Zero
            },
            SignaturePlanningAssetCoverageInCharge = new ActionSignature
            {
                AccountIdSignature = Guid.Empty,
                Alias = string.Empty,
                TglAksi = _dateTimeService.Zero
            },
            ProsesValidasi = new ValidationProcess
            {
                SignatureChatCallMulai = new ActionSignature
                {
                    AccountIdSignature = Guid.Empty,
                    Alias = string.Empty,
                    TglAksi = _dateTimeService.Zero
                },
                SignatureChatCallRespons = new ActionSignature
                {
                    AccountIdSignature = Guid.Empty,
                    Alias = string.Empty,
                    TglAksi = _dateTimeService.Zero
                },
                WaktuTanggalRespons = _dateTimeService.Zero,
                LinkChatHistory = string.Empty,
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
                StatusApproval = ApprovalStatus.OnProgress,
                RootCause = string.Empty,
                Keterangan = string.Empty,
                JarakShareLoc = 0,
                JarakICrmPlus = 0,
                VaTerbit = _dateTimeService.Zero
            },
            LastModified = _dateTimeService.DateTimeOffsetNow
        };
    }
}
