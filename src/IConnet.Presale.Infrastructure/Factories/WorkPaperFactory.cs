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
                KantorPerwakilan = importModel.KantorPerwakilan.ToUpper(),
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
            SignatureVerifikasiImport = ActionSignature.Empty(),
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
            SignatureHelpdeskInCharge = ActionSignature.Empty(),
            SignaturePlanningAssetCoverageInCharge = ActionSignature.Empty(),
            ProsesValidasi = new ValidationProcess
            {
                SignatureChatCallMulai = ActionSignature.Empty(),
                SignatureChatCallRespons = ActionSignature.Empty(),
                WaktuTanggalRespons = _dateTimeService.Zero,
                LinkChatHistory = string.Empty,
                ParameterValidasi = new ValidationParameter
                {
                    ValidasiIdPln = ValidationStatus.MenungguValidasi,
                    ValidasiNama = ValidationStatus.MenungguValidasi,
                    ValidasiNomorTelepon = ValidationStatus.MenungguValidasi,
                    ValidasiEmail = ValidationStatus.MenungguValidasi,
                    ValidasiAlamat = ValidationStatus.MenungguValidasi,
                    ShareLoc = new Coordinate()
                },
                PembetulanValidasi = new ValidationCorrection
                {
                    PembetulanIdPln = string.Empty,
                    PembetulanNama = string.Empty,
                    PembetulanNomorTelepon = string.Empty,
                    PembetulanEmail  = string.Empty,
                    PembetulanAlamat = string.Empty,
                },
                Keterangan = string.Empty
            },
            ProsesApproval = new ApprovalProcess
            {
                SignatureApproval = ActionSignature.Empty(),
                StatusApproval = ApprovalStatus.InProgress,
                DirectApproval = string.Empty,
                RootCause = string.Empty,
                Keterangan = string.Empty,
                JarakShareLoc = 0,
                JarakICrmPlus = 0,
                SplitterGanti = string.Empty,
                VaTerbit = _dateTimeService.Zero
            },
            LastModified = _dateTimeService.DateTimeOffsetNow
        };
    }

    internal WorkPaper TransformWorkPaperFromModel(IWorkPaperModel model)
    {
        return new WorkPaper
        {
            WorkPaperId = model.WorkPaperId,
            WorkPaperLevel = (WorkPaperLevel)model.WorkPaperLevel,
            Shift = model.Shift,
            SignatureHelpdeskInCharge = new ActionSignature
            {
                AccountIdSignature = model.HelpdeskInChargeAccountIdSignature,
                Alias = model.HelpdeskInChargeAliasSignature,
                TglAksi = model.HelpdeskInChargeTglAksiSignature
            },
            SignaturePlanningAssetCoverageInCharge = new ActionSignature
            {
                AccountIdSignature = model.PlanningAssetCoverageInChargeAccountIdSignature,
                Alias = model.PlanningAssetCoverageInChargeAliasSignature,
                TglAksi = model.PlanningAssetCoverageInChargeTglAksiSignature
            },
            ProsesValidasi = new ValidationProcess
            {
                SignatureChatCallMulai = new ActionSignature
                {
                    AccountIdSignature = model.ChatCallMulaiAccountIdSignature,
                    Alias = model.ChatCallMulaiAliasSignature,
                    TglAksi = model.ChatCallMulaiTglAksiSignature
                },
                SignatureChatCallRespons = new ActionSignature
                {
                    AccountIdSignature = model.ChatCallResponsAccountIdSignature,
                    Alias = model.ChatCallResponsAliasSignature,
                    TglAksi = model.ChatCallResponsTglAksiSignature
                },
                WaktuTanggalRespons = model.WaktuTanggalRespons,
                LinkChatHistory = model.LinkChatHistory,
                ParameterValidasi = new ValidationParameter
                {
                    ValidasiIdPln = (ValidationStatus)model.ValidasiIdPln,
                    ValidasiNama = (ValidationStatus)model.ValidasiNama,
                    ValidasiNomorTelepon = (ValidationStatus)model.ValidasiNomorTelepon,
                    ValidasiEmail = (ValidationStatus)model.ValidasiEmail,
                    ValidasiAlamat = (ValidationStatus)model.ValidasiAlamat,
                    ShareLoc = new Coordinate
                    {
                        Latitude = model.ShareLocLatitude,
                        Longitude = model.ShareLocLongitude
                    }
                },
                PembetulanValidasi = new ValidationCorrection
                {
                    PembetulanIdPln = model.PembetulanIdPln,
                    PembetulanNama = model.PembetulanNama,
                    PembetulanNomorTelepon = model.PembetulanNomorTelepon,
                    PembetulanEmail  = model.PembetulanEmail,
                    PembetulanAlamat = model.PembetulanAlamat,
                },
                Keterangan = model.KeteranganValidasi
            },
            ProsesApproval = new ApprovalProcess
            {
                SignatureApproval = new ActionSignature
                {
                    AccountIdSignature = model.ApprovalAccountIdSignature,
                    Alias = model.ApprovalAliasSignature,
                    TglAksi = model.ApprovalTglAksiSignature
                },
                StatusApproval = (ApprovalStatus)model.StatusApproval,
                DirectApproval = model.DirectApproval,
                RootCause = model.RootCause,
                Keterangan = model.KeteranganApproval,
                JarakShareLoc = model.JarakShareLoc,
                JarakICrmPlus = model.JarakICrmPlus,
                SplitterGanti = model.SplitterGanti,
                VaTerbit = model.VaTerbit
            },
            FkApprovalOpportunityId = model.ApprovalOpportunityId,
            ApprovalOpportunity = new ApprovalOpportunity
            {
                IdPermohonan = model.IdPermohonan,
                TglPermohonan = model.TglPermohonan,
                StatusPermohonan = model.StatusPermohonan,
                Layanan = model.Layanan,
                SumberPermohonan = model.SumberPermohonan,
                JenisPermohonan = model.JenisPermohonan,
                Splitter = model.Splitter,
                Pemohon = new Applicant
                {
                    IdPln = model.IdPlnPelanggan,
                    NamaPelanggan = model.NamaPelanggan,
                    NomorTelepon = model.NomorTeleponPelanggan,
                    Email = model.EmailPelanggan,
                    Alamat = model.AlamatPelanggan,
                    Nik = model.NikPelanggan,
                    Npwp = model.NpwpPelanggan,
                    Keterangan = model.KeteranganPelanggan
                },
                Agen = new Salesperson
                {
                    NamaLengkap = model.NamaAgen,
                    Email = model.EmailAgen,
                    NomorTelepon = model.NomorTeleponAgen,
                    Mitra = model.MitraAgen
                },
                Regional = new Regional
                {
                    Bagian = model.Bagian,
                    KantorPerwakilan = model.KantorPerwakilan,
                    Provinsi = model.Provinsi,
                    Kabupaten = model.Kabupaten,
                    Kecamatan = model.Kecamatan,
                    Kelurahan = model.Kelurahan,
                    Koordinat = new Coordinate
                    {
                        Latitude = model.KoordinatLatitude,
                        Longitude = model.KoordinatLongitude
                    }
                },
                StatusImport = (ImportStatus)model.StatusImport,
                SignatureImport = new ActionSignature
                {
                    AccountIdSignature = model.ImportAccountIdSignature,
                    Alias = model.ImportAliasSignature,
                    TglAksi = model.ImportTglAksiSignature,

                },
                SignatureVerifikasiImport = new ActionSignature
                {
                    AccountIdSignature = model.VerifikasiImportAccountIdSignature,
                    Alias = model.VerifikasiImportAliasSignature,
                    TglAksi = model.VerifikasiImportTglAksiSignature
                }
            },
            LastModified = _dateTimeService.DateTimeOffsetNow
        };
    }
}
