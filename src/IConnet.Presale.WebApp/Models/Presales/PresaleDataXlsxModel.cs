namespace IConnet.Presale.WebApp.Models.Presales;

public class PresaleDataXlsxModel
{
    public PresaleDataXlsxModel(WorkPaper workPaper)
    {
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
        RegionalBagian = workPaper.ApprovalOpportunity.Regional.Bagian;
        KantorPerwakilan = workPaper.ApprovalOpportunity.Regional.KantorPerwakilan;
        Provinsi = workPaper.ApprovalOpportunity.Regional.Provinsi;
        Kabupaten = workPaper.ApprovalOpportunity.Regional.Kabupaten;
        Kecamatan = workPaper.ApprovalOpportunity.Regional.Kecamatan;
        Kelurahan = workPaper.ApprovalOpportunity.Regional.Kelurahan;
        Koordinat = workPaper.ApprovalOpportunity.Regional.Koordinat.GetLatitudeLongitude();
        KoordinatLatitude = workPaper.ApprovalOpportunity.Regional.Koordinat.Latitude;
        KoordinatLongitude = workPaper.ApprovalOpportunity.Regional.Koordinat.Longitude;

        Shift = workPaper.Shift;
        WaktuTanggalRespons = workPaper.ProsesValidasi.WaktuTanggalRespons;
        LinkChatHistory = workPaper.ProsesValidasi.LinkChatHistory;
        ValidasiIdPln = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiIdPln);
        ValidasiNama = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNama);
        ValidasiNomorTelepon = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiNomorTelepon);
        ValidasiEmail = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiEmail);
        ValidasiAlamat = EnumProcessor.EnumToDisplayString(workPaper.ProsesValidasi.ParameterValidasi.ValidasiAlamat);
        ShareLoc = workPaper.ProsesValidasi.ParameterValidasi.ShareLoc.GetLatitudeLongitude();
        ShareLocLatitude = workPaper.ProsesValidasi.ParameterValidasi.ShareLoc.Latitude;
        ShareLocLongitude = workPaper.ProsesValidasi.ParameterValidasi.ShareLoc.Longitude;
        PembetulanIdPln = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanIdPln;
        PembetulanNama = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanNama;
        PembetulanNomorTelepon = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanNomorTelepon;
        PembetulanEmail = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanEmail;
        PembetulanAlamat = workPaper.ProsesValidasi.PembetulanValidasi.PembetulanAlamat;
        KeteranganValidasi = workPaper.ProsesValidasi.Keterangan;

        StatusApproval = EnumProcessor.EnumToDisplayString(workPaper.ProsesApproval.StatusApproval);
        DirectApproval = workPaper.ProsesApproval.DirectApproval;
        RootCause = workPaper.ProsesApproval.RootCause.ToUpper();
        KeteranganApproval = workPaper.ProsesApproval.Keterangan;
        JarakShareLoc = workPaper.ProsesApproval.JarakShareLoc;
        JarakICrmPlus = workPaper.ProsesApproval.JarakICrmPlus;
        SplitterGanti = workPaper.ProsesApproval.SplitterGanti;
        VaTerbit = workPaper.ProsesApproval.VaTerbit;
    }

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
    public string RegionalBagian { get; init; }
    public string KantorPerwakilan { get; init; }
    public string Provinsi { get; init; }
    public string Kabupaten { get; init; }
    public string Kecamatan { get; init; }
    public string Kelurahan { get; init; }
    public string Koordinat { get; init; }
    public string KoordinatLatitude { get; init; }
    public string KoordinatLongitude { get; init; }

    public string Shift { get; init; }
    public DateTime WaktuTanggalRespons { get; init; }
    public string LinkChatHistory { get; init; }
    public string ValidasiIdPln { get; init; }
    public string ValidasiNama { get; init; }
    public string ValidasiNomorTelepon { get; init; }
    public string ValidasiEmail { get; init; }
    public string ValidasiAlamat { get; init; }
    public string ShareLoc { get; init; }
    public string ShareLocLatitude { get; init; }
    public string ShareLocLongitude { get; init; }
    public string PembetulanIdPln { get; init; }
    public string PembetulanNama { get; init; }
    public string PembetulanNomorTelepon { get; init; }
    public string PembetulanEmail { get; init; }
    public string PembetulanAlamat { get; init; }
    public string KeteranganValidasi { get; init; }

    public string StatusApproval { get; init; }
    public string DirectApproval { get; init; }
    public string RootCause { get; init; }
    public string KeteranganApproval { get; init; }
    public int JarakShareLoc { get; init; }
    public int JarakICrmPlus { get; init; }
    public string SplitterGanti { get; init; }
    public DateTime VaTerbit { get; init; }

    public string GetDirectApproval()
    {
        return DirectApproval.IsNullOrWhiteSpace()
            ? "NORMAL"
            : DirectApproval;
    }

    public  DateTime? GetWaktuTanggalRespons()
    {
        return WaktuTanggalRespons == DateTime.MinValue
            ? null
            : WaktuTanggalRespons;
    }

    public DateTime? GetVaTerbit()
    {
        return VaTerbit == DateTime.MinValue
            ? null
            : VaTerbit;
    }
}
