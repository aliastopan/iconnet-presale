using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Shared.Contracts.Presale;

public record InsertWorkPaperRequest : IWorkPaperModel
{
    public InsertWorkPaperRequest(
        Guid workPaperId,
        int workPaperLevel,
        string shift,
        Guid helpdeskInChargeAccountIdSignature,
        string helpdeskInChargeAliasSignature,
        DateTime helpdeskInChargeTglAksiSignature,
        Guid planningAssetCoverageInChargeAccountIdSignature,
        string planningAssetCoverageInChargeAliasSignature,
        DateTime planningAssetCoverageInChargeTglAksiSignature,
        Guid chatCallMulaiAccountIdSignature,
        string chatCallMulaiAliasSignature,
        DateTime chatCallMulaiTglAksiSignature,
        Guid chatCallResponsAccountIdSignature,
        string chatCallResponsAliasSignature,
        DateTime chatCallResponsTglAksiSignature,
        DateTime waktuTanggalRespons,
        string linkChatHistory,
        int validasiIdPln,
        int validasiNama,
        int validasiNomorTelepon,
        int validasiEmail,
        int validasiAlamat,
        string shareLocLatitude,
        string shareLocLongitude,
        string pembetulanIdPln,
        string pembetulanNama,
        string pembetulanNomorTelepon,
        string pembetulanEmail,
        string pembetulanAlamat,
        string keteranganValidasi,
        Guid approvalAccountIdSignature,
        string approvalAliasSignature,
        DateTime approvalTglAksiSignature,
        int statusApproval,
        string directApproval,
        string rootCause,
        string keteranganApproval,
        int jarakShareLoc,
        int jarakICrmPlus,
        string splitterGanti,
        DateTime vaTerbit,
        Guid approvalOpportunityId,
        string idPermohonan,
        DateTime tglPermohonan,
        string statusPermohonan,
        string layanan,
        string sumberPermohonan,
        string jenisPermohonan,
        string splitter,
        string idPlnPelanggan,
        string namaPelanggan,
        string nomorTeleponPelanggan,
        string emailPelanggan,
        string alamatPelanggan,
        string nikPelanggan,
        string npwpPelanggan,
        string keteranganPelanggan,
        string namaAgen,
        string emailAgen,
        string nomorTeleponAgen,
        string mitraAgen,
        string bagian,
        string kantorPerwakilan,
        string provinsi,
        string kabupaten,
        string kecamatan,
        string kelurahan,
        string koordinatLatitude,
        string koordinatLongitude,
        int statusImport,
        Guid importAccountIdSignature,
        string importAliasSignature,
        DateTime importTglAksiSignature,
        Guid verifikasiImportAccountIdSignature,
        string verifikasiImportAliasSignature,
        DateTime verifikasiImportTglAksiSignature
    )
    {
        WorkPaperId = workPaperId;
        WorkPaperLevel = workPaperLevel;
        Shift = shift;
        HelpdeskInChargeAccountIdSignature = helpdeskInChargeAccountIdSignature;
        HelpdeskInChargeAliasSignature = helpdeskInChargeAliasSignature;
        HelpdeskInChargeTglAksiSignature = helpdeskInChargeTglAksiSignature;
        PlanningAssetCoverageInChargeAccountIdSignature = planningAssetCoverageInChargeAccountIdSignature;
        PlanningAssetCoverageInChargeAliasSignature = planningAssetCoverageInChargeAliasSignature;
        PlanningAssetCoverageInChargeTglAksiSignature = planningAssetCoverageInChargeTglAksiSignature;
        ChatCallMulaiAccountIdSignature = chatCallMulaiAccountIdSignature;
        ChatCallMulaiAliasSignature = chatCallMulaiAliasSignature;
        ChatCallMulaiTglAksiSignature = chatCallMulaiTglAksiSignature;
        ChatCallResponsAccountIdSignature = chatCallResponsAccountIdSignature;
        ChatCallResponsAliasSignature = chatCallResponsAliasSignature;
        ChatCallResponsTglAksiSignature = chatCallResponsTglAksiSignature;
        WaktuTanggalRespons = waktuTanggalRespons;
        LinkChatHistory = linkChatHistory;
        ValidasiIdPln = validasiIdPln;
        ValidasiNama = validasiNama;
        ValidasiNomorTelepon = validasiNomorTelepon;
        ValidasiEmail = validasiEmail;
        ValidasiAlamat = validasiAlamat;
        ShareLocLatitude = shareLocLatitude;
        ShareLocLongitude = shareLocLongitude;
        PembetulanIdPln = pembetulanIdPln;
        PembetulanNama = pembetulanNama;
        PembetulanNomorTelepon = pembetulanNomorTelepon;
        PembetulanEmail = pembetulanEmail;
        PembetulanAlamat = pembetulanAlamat;
        KeteranganValidasi = keteranganValidasi;
        ApprovalAccountIdSignature = approvalAccountIdSignature;
        ApprovalAliasSignature = approvalAliasSignature;
        ApprovalTglAksiSignature = approvalTglAksiSignature;
        StatusApproval = statusApproval;
        DirectApproval = directApproval;
        RootCause = rootCause;
        KeteranganApproval = keteranganApproval;
        JarakShareLoc = jarakShareLoc;
        JarakICrmPlus = jarakICrmPlus;
        SplitterGanti = splitterGanti;
        VaTerbit = vaTerbit;
        ApprovalOpportunityId = approvalOpportunityId;
        IdPermohonan = idPermohonan;
        TglPermohonan = tglPermohonan;
        StatusPermohonan = statusPermohonan;
        Layanan = layanan;
        SumberPermohonan = sumberPermohonan;
        JenisPermohonan = jenisPermohonan;
        Splitter = splitter;
        IdPlnPelanggan = idPlnPelanggan;
        NamaPelanggan = namaPelanggan;
        NomorTeleponPelanggan = nomorTeleponPelanggan;
        EmailPelanggan = emailPelanggan;
        AlamatPelanggan = alamatPelanggan;
        NikPelanggan = nikPelanggan;
        NpwpPelanggan = npwpPelanggan;
        KeteranganPelanggan = keteranganPelanggan;
        NamaAgen = namaAgen;
        EmailAgen = emailAgen;
        NomorTeleponAgen = nomorTeleponAgen;
        MitraAgen = mitraAgen;
        Bagian = bagian;
        KantorPerwakilan = kantorPerwakilan;
        Provinsi = provinsi;
        Kabupaten = kabupaten;
        Kecamatan = kecamatan;
        Kelurahan = kelurahan;
        KoordinatLatitude = koordinatLatitude;
        KoordinatLongitude = koordinatLongitude;
        StatusImport = statusImport;
        ImportAccountIdSignature = importAccountIdSignature;
        ImportAliasSignature = importAliasSignature;
        ImportTglAksiSignature = importTglAksiSignature;
        VerifikasiImportAccountIdSignature = verifikasiImportAccountIdSignature;
        VerifikasiImportAliasSignature = verifikasiImportAliasSignature;
        VerifikasiImportTglAksiSignature = verifikasiImportTglAksiSignature;
    }

    public Guid WorkPaperId { get; init; }
    public int WorkPaperLevel { get; init; }
    public string Shift { get; init; }
    public Guid HelpdeskInChargeAccountIdSignature { get; init; }
    public string HelpdeskInChargeAliasSignature { get; init; }
    public DateTime HelpdeskInChargeTglAksiSignature { get; init; }
    public Guid PlanningAssetCoverageInChargeAccountIdSignature { get; init; }
    public string PlanningAssetCoverageInChargeAliasSignature { get; init; }
    public DateTime PlanningAssetCoverageInChargeTglAksiSignature { get; init; }

    public Guid ChatCallMulaiAccountIdSignature { get; init; }
    public string ChatCallMulaiAliasSignature { get; init; }
    public DateTime ChatCallMulaiTglAksiSignature { get; init; }
    public Guid ChatCallResponsAccountIdSignature { get; init; }
    public string ChatCallResponsAliasSignature { get; init; }
    public DateTime ChatCallResponsTglAksiSignature { get; init; }
    public DateTime WaktuTanggalRespons { get; init; }
    public string LinkChatHistory { get; init; }
    public int ValidasiIdPln { get; init; }
    public int ValidasiNama { get; init; }
    public int ValidasiNomorTelepon { get; init; }
    public int ValidasiEmail { get; init; }
    public int ValidasiAlamat { get; init; }
    public string ShareLocLatitude { get; init; }
    public string ShareLocLongitude { get; init; }
    public string PembetulanIdPln { get; init; }
    public string PembetulanNama { get; init; }
    public string PembetulanNomorTelepon { get; init; }
    public string PembetulanEmail { get; init; }
    public string PembetulanAlamat { get; init; }
    public string KeteranganValidasi { get; init; }

    public Guid ApprovalAccountIdSignature { get; init; }
    public string ApprovalAliasSignature { get; init; }
    public DateTime ApprovalTglAksiSignature { get; init; }
    public int StatusApproval { get; init; }
    public string DirectApproval { get; init; }
    public string RootCause { get; init; }
    public string KeteranganApproval { get; init; }
    public int JarakShareLoc { get; init; }
    public int JarakICrmPlus { get; init; }
    public string SplitterGanti { get; init; }
    public DateTime VaTerbit { get; init; }

    public Guid ApprovalOpportunityId { get; init; }
    public string IdPermohonan { get; init; }
    public DateTime TglPermohonan { get; init; }
    public string StatusPermohonan { get; init; }
    public string Layanan { get; init; }
    public string SumberPermohonan { get; init; }
    public string JenisPermohonan { get; init; }
    public string Splitter { get; init; }
    public string IdPlnPelanggan { get; init; }
    public string NamaPelanggan { get; init; }
    public string NomorTeleponPelanggan { get; init; }
    public string EmailPelanggan { get; init; }
    public string AlamatPelanggan { get; init; }
    public string NikPelanggan { get; init; }
    public string NpwpPelanggan { get; init; }
    public string KeteranganPelanggan { get; init; }
    public string NamaAgen { get; init; }
    public string EmailAgen { get; init; }
    public string NomorTeleponAgen { get; init; }
    public string MitraAgen { get; init; }
    public string Bagian { get; init; }
    public string KantorPerwakilan { get; init; }
    public string Provinsi { get; init; }
    public string Kabupaten { get; init; }
    public string Kecamatan { get; init; }
    public string Kelurahan { get; init; }
    public string KoordinatLatitude { get; init; }
    public string KoordinatLongitude { get; init; }
    public int StatusImport { get; init; }
    public Guid ImportAccountIdSignature { get; init; }
    public string ImportAliasSignature { get; init; }
    public DateTime ImportTglAksiSignature { get; init; }
    public Guid VerifikasiImportAccountIdSignature { get; init; }
    public string VerifikasiImportAliasSignature { get; init; }
    public DateTime VerifikasiImportTglAksiSignature { get; init; }
}
