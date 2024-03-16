namespace IConnet.Presale.WebApp.Models.Presales;

public class WorkPaperFlatJsonModel : IWorkPaperModel
{
    public WorkPaperFlatJsonModel(WorkPaper workPaper)
    {
        WorkPaperId = workPaper.WorkPaperId;
        WorkPaperLevel = (int)workPaper.WorkPaperLevel;
        Shift = workPaper.Shift;
        HelpdeskInChargeAccountIdSignature = workPaper.SignatureHelpdeskInCharge.AccountIdSignature;
        HelpdeskInChargeAliasSignature = workPaper.SignatureHelpdeskInCharge.Alias;
        HelpdeskInChargeTglAksiSignature = workPaper.SignatureHelpdeskInCharge.TglAksi;
        PlanningAssetCoverageInChargeAccountIdSignature = workPaper.SignaturePlanningAssetCoverageInCharge.AccountIdSignature;
        PlanningAssetCoverageInChargeAliasSignature = workPaper.SignaturePlanningAssetCoverageInCharge.Alias;
        PlanningAssetCoverageInChargeTglAksiSignature = workPaper.SignaturePlanningAssetCoverageInCharge.TglAksi;

        ChatCallMulaiAccountIdSignature = workPaper.ProsesValidasi.SignatureChatCallMulai.AccountIdSignature;
        ChatCallMulaiAliasSignature = workPaper.ProsesValidasi.SignatureChatCallMulai.Alias;
        ChatCallMulaiTglAksiSignature = workPaper.ProsesValidasi.SignatureChatCallMulai.TglAksi;
        ChatCallResponsAccountIdSignature = workPaper.ProsesValidasi.SignatureChatCallRespons.AccountIdSignature;
        ChatCallResponsAliasSignature = workPaper.ProsesValidasi.SignatureChatCallRespons.Alias;
        ChatCallResponsTglAksiSignature = workPaper.ProsesValidasi.SignatureChatCallRespons.TglAksi;
        WaktuTanggalRespons = workPaper.ProsesValidasi.WaktuTanggalRespons;
        LinkChatHistory = workPaper.ProsesValidasi.LinkChatHistory;
        ValidasiIdPln = (int)workPaper.ProsesValidasi.ParameterValidasi.ValidasiIdPln;
        ValidasiNama = (int)workPaper.ProsesValidasi.ParameterValidasi.ValidasiNama;
        ValidasiNomorTelepon = (int)workPaper.ProsesValidasi.ParameterValidasi.ValidasiNomorTelepon;
        ValidasiEmail = (int)workPaper.ProsesValidasi.ParameterValidasi.ValidasiEmail;
        ValidasiAlamat = (int)workPaper.ProsesValidasi.ParameterValidasi.ValidasiAlamat;
        ShareLocLatitude = workPaper.ProsesValidasi.ParameterValidasi.ShareLoc.Latitude;
        ShareLocLongitude = workPaper.ProsesValidasi.ParameterValidasi.ShareLoc.Longitude;
        PembetulanIdPln = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanIdPln;
        PembetulanNama = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanNama;
        PembetulanNomorTelepon = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanNomorTelepon;
        PembetulanEmail = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanEmail;
        PembetulanAlamat = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanAlamat;
        KeteranganValidasi = workPaper.ProsesValidasi.Keterangan;

        ApprovalAccountIdSignature = workPaper.ProsesApproval.SignatureApproval.AccountIdSignature;
        ApprovalAliasSignature = workPaper.ProsesApproval.SignatureApproval.Alias;
        ApprovalTglAksiSignature = workPaper.ProsesApproval.SignatureApproval.TglAksi;
        StatusApproval = (int)workPaper.ProsesApproval.StatusApproval;
        RootCause = workPaper.ProsesApproval.RootCause;
        KeteranganApproval = workPaper.ProsesApproval.Keterangan;
        JarakShareLoc = workPaper.ProsesApproval.JarakShareLoc;
        JarakICrmPlus = workPaper.ProsesApproval.JarakICrmPlus;
        VaTerbit = workPaper.ProsesApproval.VaTerbit;

        ApprovalOpportunityId = workPaper.ApprovalOpportunity.ApprovalOpportunityId;
        IdPermohonan = workPaper.ApprovalOpportunity.IdPermohonan;
        TglPermohonan = workPaper.ApprovalOpportunity.TglPermohonan;
        StatusPermohonan = workPaper.ApprovalOpportunity.StatusPermohonan;
        Layanan = workPaper.ApprovalOpportunity.Layanan;
        SumberPermohonan = workPaper.ApprovalOpportunity.SumberPermohonan;
        JenisPermohonan = workPaper.ApprovalOpportunity.JenisPermohonan;
        Splitter = workPaper.ApprovalOpportunity.Splitter;
        IdPlnPelanggan = workPaper.ApprovalOpportunity.Pemohon.IdPln;
        NamaPelanggan = workPaper.ApprovalOpportunity.Pemohon.NamaPelanggan;
        NomorTeleponPelanggan = workPaper.ApprovalOpportunity.Pemohon.NomorTelepon;
        EmailPelanggan = workPaper.ApprovalOpportunity.Pemohon.Email;
        AlamatPelanggan = workPaper.ApprovalOpportunity.Pemohon.Alamat;
        NikPelanggan = workPaper.ApprovalOpportunity.Pemohon.Nik;
        NpwpPelanggan = workPaper.ApprovalOpportunity.Pemohon.Npwp;
        KeteranganPelanggan = workPaper.ApprovalOpportunity.Pemohon.Keterangan;
        NamaAgen = workPaper.ApprovalOpportunity.Agen.NamaLengkap;
        EmailAgen = workPaper.ApprovalOpportunity.Agen.Email;
        NomorTeleponAgen = workPaper.ApprovalOpportunity.Agen.NomorTelepon;
        MitraAgen = workPaper.ApprovalOpportunity.Agen.Mitra;
        Bagian = workPaper.ApprovalOpportunity.Regional.Bagian;
        KantorPerwakilan = workPaper.ApprovalOpportunity.Regional.KantorPerwakilan;
        Provinsi = workPaper.ApprovalOpportunity.Regional.Provinsi;
        Kabupaten = workPaper.ApprovalOpportunity.Regional.Kabupaten;
        Kecamatan = workPaper.ApprovalOpportunity.Regional.Kecamatan;
        Kelurahan = workPaper.ApprovalOpportunity.Regional.Kelurahan;
        KoordinatLatitude = workPaper.ApprovalOpportunity.Regional.Koordinat.Latitude;
        KoordinatLongitude = workPaper.ApprovalOpportunity.Regional.Koordinat.Longitude;

        StatusImport = (int)workPaper.ApprovalOpportunity.StatusImport;
        ImportAccountIdSignature = workPaper.ApprovalOpportunity.SignatureImport.AccountIdSignature;
        ImportAliasSignature = workPaper.ApprovalOpportunity.SignatureImport.Alias;
        ImportTglAksiSignature = workPaper.ApprovalOpportunity.SignatureImport.TglAksi;
        VerifikasiImportAccountIdSignature = workPaper.ApprovalOpportunity.SignatureVerifikasiImport.AccountIdSignature;
        VerifikasiImportAliasSignature = workPaper.ApprovalOpportunity.SignatureVerifikasiImport.Alias;
        VerifikasiImportTglAksiSignature = workPaper.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi;
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
    public string RootCause { get; init; }
    public string KeteranganApproval { get; init; }
    public int JarakShareLoc { get; init; }
    public int JarakICrmPlus { get; init; }
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
