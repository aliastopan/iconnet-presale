namespace IConnet.Presale.WebApp.Extensions;

public static class WorkPaperSortingExtensions
{
    public static GridSort<WorkPaper> SortByWorkPaperLevel(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.WorkPaperLevel);
    }

    public static GridSort<WorkPaper> SortByIdPermohonan(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.IdPermohonan);
    }

    public static GridSort<WorkPaper> SortByTglPermohonan(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.TglPermohonan);
    }

    public static GridSort<WorkPaper> SortByNamaPemohon(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Pemohon.NamaPelanggan);
    }

    public static GridSort<WorkPaper> SortByNomorTeleponPemohon(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Pemohon.NomorTelepon);
    }

    public static GridSort<WorkPaper> SortByEmailPemohon(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Pemohon.Email);
    }

    public static GridSort<WorkPaper> SortByIdPln(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Pemohon.IdPln);
    }

    public static GridSort<WorkPaper> SortByAlamatPemohon(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Pemohon.Alamat);
    }

    public static GridSort<WorkPaper> SortByNikPemohon(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Pemohon.Nik);
    }

    public static GridSort<WorkPaper> SortByNpwpPemohon(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Pemohon.Npwp);
    }

    public static GridSort<WorkPaper> SortByTglChatCallMulai(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ProsesValidasi.SignatureChatCallMulai.TglAksi);
    }

    public static GridSort<WorkPaper> SortByTglChatCallRespons(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ProsesValidasi.SignatureChatCallRespons.TglAksi);
    }

    public static GridSort<WorkPaper> SortByHelpdeskInCharge(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.SignatureHelpdeskInCharge.Alias);
    }

    public static GridSort<WorkPaper> SortByStatusApproval(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ProsesApproval.StatusApproval);
    }

    public static GridSort<WorkPaper> SortByRootCause(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ProsesApproval.RootCause);
    }

    public static GridSort<WorkPaper> SortByTglApproval(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ProsesApproval.SignatureApproval.TglAksi);
    }

    public static GridSort<WorkPaper> SortByPlanningAssetCoverageInCharge(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ProsesApproval.SignatureApproval.Alias);
    }

    public static GridSort<WorkPaper> SortByLayanan(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Layanan);
    }

    public static GridSort<WorkPaper> SortBySumberPermohonan(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.SumberPermohonan);
    }

    public static GridSort<WorkPaper> SortByNamaAgen(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Agen.NamaLengkap);
    }

    public static GridSort<WorkPaper> SortByEmailAgen(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Agen.Email);
    }

    public static GridSort<WorkPaper> SortByNomorTeleponAgen(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Agen.NomorTelepon);
    }

    public static GridSort<WorkPaper> SortByMitraAgen(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Agen.Mitra);
    }

    public static GridSort<WorkPaper> SortBySplitter(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Splitter);
    }

    public static GridSort<WorkPaper> SortByRegional(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Regional.Bagian);
    }

    public static GridSort<WorkPaper> SortByKantorPerwakilan(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Regional.KantorPerwakilan);
    }

    public static GridSort<WorkPaper> SortByProvinsi(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Regional.Provinsi);
    }

    public static GridSort<WorkPaper> SortByKabupaten(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Regional.Kabupaten);
    }

    public static GridSort<WorkPaper> SortByKecamatan(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Regional.Kecamatan);
    }

    public static GridSort<WorkPaper> SortByKelurahan(this IQueryable<WorkPaper>? _)
    {
        return GridSort<WorkPaper>.ByAscending(workPaper => workPaper.ApprovalOpportunity.Regional.Kelurahan);
    }
}
