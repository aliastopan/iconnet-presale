using System.Text.Json;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class WorkloadManager : IWorkloadManager
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ICacheService _cacheService;

    public WorkloadManager(IDateTimeService dateTimeService,
        ICacheService cacheService)
    {
        _dateTimeService = dateTimeService;
        _cacheService = cacheService;
    }

    public async Task<int> CacheWorkloadAsync(List<IApprovalOpportunityModel> importModels)
    {
        int workloadCount = 0;

        foreach (var importModel in importModels)
        {
            var workPaper = CreateWorkPaper(importModel);

            var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
            var key = workPaper.ApprovalOpportunity.IdPermohonan;

            var isKeyExists = await _cacheService.IsKeyExistsAsync(key);
            if (!isKeyExists)
            {
                await _cacheService.SetCacheValueAsync(key, jsonWorkPaper);
                workloadCount++;
            }
        }

        return workloadCount;
    }

    public async Task<List<WorkPaper>> FetchWorkloadAsync(bool onlyVerified = true)
    {
        List<WorkPaper> workPapers = [];
        List<string?> jsonWorkPapers = await _cacheService.GetAllCacheValuesAsync();

        foreach (var json in jsonWorkPapers)
        {
            if (json is null)
            {
                continue;
            }

            var workPaper = JsonSerializer.Deserialize<WorkPaper>(json)!;

            if (onlyVerified && workPaper.StatusImport != ImportStatus.Verified)
            {
                continue;
            }

            workPapers.Add(workPaper);
        }

        return workPapers;
    }

    public WorkPaper CreateWorkPaper(IApprovalOpportunityModel importModel)
    {
        var approvalOpportunity = CreateApprovalOpportunity(importModel);
        return new WorkPaper
        {
            FkApprovalOpportunityId = approvalOpportunity.ApprovalOpportunityId,
            ApprovalOpportunity = approvalOpportunity,
            Shift = "",
            PersonInCharge = new PersonInCharge
            {
                Helpdesk = "",
                PlanningAssetCoverage = ""
            },
            ProsesValidasi = new ValidationProcess
            {
                TglChatCallMulai = _dateTimeService.DateTimeOffsetNow.DateTime,
                TglChatCallRespons = _dateTimeService.DateTimeOffsetNow.DateTime,
                LinkRecapChatHistory = "",
                ParameterValidasi = new ValidationParameter
                {
                    PlnId = ValidationStatus.Unset,
                    NamaPelanggan = ValidationStatus.Unset,
                    NomorTelepon = ValidationStatus.Unset,
                    Email = ValidationStatus.Unset,
                    AlamatPelanggan = ValidationStatus.Unset,
                    ShareLoc = new Coordinate
                    {
                        Latitude = "",
                        Longitude = ""
                    }
                },
                StatusValidasi = ValidationStatus.Unset,
                Keterangan = ""
            },
            ProsesApproval = new ApprovalProcess
            {
                StatusApproval = "",
                RootCause = "",
                Keterangan = "",
                JarakShareLoc = "",
                JarakICrmPlus = "",
                VaTerbit = _dateTimeService.Zero
            },
            StatusImport = ImportStatus.Unverified
        };
    }

    public ApprovalOpportunity CreateApprovalOpportunity(IApprovalOpportunityModel importModel)
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
            TglImport = importModel.TglImport.DateTime,
            NamaClaimImport = importModel.NamaClaimImport
        };
    }

    public async Task<bool> ClaimWorkPaperAsync(string cacheKey, string claimName)
    {
        var isWorkPaperExist = await _cacheService.IsKeyExistsAsync(cacheKey);
        if (!isWorkPaperExist)
        {
            return false;
        }

        var jsonWorkPaper = await _cacheService.GetCacheValueAsync(cacheKey);
        var workPaper = JsonSerializer.Deserialize<WorkPaper>(jsonWorkPaper!);

        workPaper!.PersonInCharge.Helpdesk = claimName;

        jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
        await _cacheService.SetCacheValueAsync(cacheKey, jsonWorkPaper);

        return true;
    }
}
