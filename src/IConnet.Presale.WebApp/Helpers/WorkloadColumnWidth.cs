namespace IConnet.Presale.WebApp.Helpers;

public class WorkloadColumnWidth : ColumnWidthBase<WorkPaper>
{
    public override void SetColumnWidth(IQueryable<WorkPaper>? workPaper)
    {
        if (workPaper is null || !workPaper.Any())
        {
            return;
        }

        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Pemohon.NamaPelanggan.Length, width => NamaPemohonPx = width, "Nama Pemohon", isCapitalized: true);
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Pemohon.Email.Length, width => EmailPemohonPx = width, "Email Pemohon");
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Pemohon.Alamat.Length, width => AlamatPemohonPx = width, "Alamat Pemohon", isCapitalized: true);
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Agen.NamaLengkap.Length, width => NamaAgenPx = width, "Nama Agen", isCapitalized: true);
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Agen.Email.Length, width => EmailAgenPx = width, "Email Agen");
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Agen.Mitra.Length, width => MitraAgenPx = width, "Mitra Agen");
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Pemohon.Keterangan.Length, width => KeteranganPx = width, "Keterangan");

        SetColumnWidth(workPaper, crm => crm.Shift.Length, width => KeteranganPx = width, "Shift");
        SetColumnWidth(workPaper, crm => crm.ProsesValidasi.ParameterValidasi.ShareLoc.GetLatitudeLongitude().Length, width => ValidasiShareLocPx = width, "Share Loc");
        SetColumnWidth(workPaper, crm => crm.ProsesValidasi.LinkRekapChatHistory.Length, width => LinkRekapChatHistoryPx = width, "Rekap Chat History");
        SetColumnWidth(workPaper, crm => crm.ProsesValidasi.Keterangan.Length, width => KeteranganValidasiPx = width, "Keterangan Validasi");
    }
}
