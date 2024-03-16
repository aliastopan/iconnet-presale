namespace IConnet.Presale.Shared.Interfaces.Models.Presales;

interface IWorkPaperModel
{
    Guid WorkPaperId { get; }
    int WorkPaperLevel { get; }
    string Shift { get; }
    Guid HelpdeskInChargeAccountIdSignature { get; }
    string HelpdeskInChargeAliasSignature { get; }
    DateTime HelpdeskInChargeTglAksiSignature { get; }
    Guid PlanningAssetCoverageInChargeAccountIdSignature { get; }
    string PlanningAssetCoverageInChargeAliasSignature { get; }
    DateTime PlanningAssetCoverageInChargeTglAksiSignature { get; }

    // ProsesValidasi
    Guid ChatCallMulaiAccountIdSignature { get; }
    string ChatCallMulaiAliasSignature { get; }
    DateTime ChatCallMulaiTglAksiSignature { get; }
    Guid ChatCallResponsAccountIdSignature { get; }
    string ChatCallResponsAliasSignature { get; }
    DateTime ChatCallResponsTglAksiSignature { get; }
    DateTime WaktuTanggalRespons { get; }
    string LinkChatHistory { get; }
    int ValidasiIdPln { get; }
    int ValidasiNama { get; }
    int ValidasiNomorTelepon { get; }
    int ValidasiEmail { get; }
    int ValidasiAlamat { get; }
    string ShareLocLatitude { get; }
    string ShareLocLongitude { get; }
    string PembetulanIdPln { get; }
    string PembetulanNama { get; }
    string PembetulanNomorTelepon { get; }
    string PembetulanEmail { get; }
    string PembetulanAlamat { get; }
    string KeteranganValidasi { get; }

    // ProsesApproval
    Guid ApprovalAccountIdSignature { get; }
    string ApprovalAliasSignature { get; }
    DateTime ApprovalTglAksiSignature { get; }
    int StatusApproval { get; }
    string RootCause { get; }
    string KeteranganApproval { get; }
    int JarakShareLoc { get; }
    int JarakICrmPlus { get; }
    DateTime VaTerbit { get; }

    // ApprovalOpportunity
    Guid ApprovalOpportunityId { get; }
    string IdPermohonan { get; }
    DateTime TglPermohonan { get; }
    string StatusPermohonan { get; }
    string Layanan { get; }
    string SumberPermohonan { get; }
    string JenisPermohonan { get; }
    string Splitter { get; }
    string IdPlnPelanggan { get; }
    string NamaPelanggan { get; }
    string NomorTeleponPelanggan { get; }
    string EmailPelanggan { get; }
    string AlamatPelanggan { get; }
    string NikPelanggan { get; }
    string NpwpPelanggan { get; }
    string KeteranganPelanggan { get; }
    string NamaAgen { get; }
    string EmailAgen { get; }
    string NomorTeleponAgen { get; }
    string MitraAgen { get; }
    int StatusImport { get; }
    Guid ImportAccountIdSignature { get; }
    string ImportAliasSignature { get; }
    DateTime ImportTglAksiSignature { get; }
    Guid VerifikasiImportAccountIdSignature { get; }
    string VerifikasiImportAliasSignature { get; }
    DateTime VerifikasiImportTglAksiSignature { get; }
}
