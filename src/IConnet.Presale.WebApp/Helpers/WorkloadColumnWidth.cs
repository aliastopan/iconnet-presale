namespace IConnet.Presale.WebApp.Helpers;

public class WorkloadColumnWidth : ColumnWidthBase<WorkPaper>
{
    public override void SetColumnWidth(IQueryable<WorkPaper>? workPaper)
    {
        if (workPaper is null || !workPaper.Any())
        {
            return;
        }

        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Pemohon.NamaLengkap.Length, width => NamaPemohonPx = width, "Nama Pemohon");
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Pemohon.Email.Length, width => EmailPemohonPx = width, "Email Pemohon");
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Pemohon.Alamat.Length, width => AlamatPemohonPx = width, "Alamat Pemohon");
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Agen.NamaLengkap.Length, width => NamaAgenPx = width, "Nama Agen");
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Agen.Email.Length, width => EmailAgenPx = width, "Email Agen");
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Agen.Mitra.Length, width => MitraAgenPx = width, "Mitra Agen");
        SetColumnWidth(workPaper, crm => crm.ApprovalOpportunity.Pemohon.Keterangan.Length, width => KeteranganPx = width, "Keterangan");
    }
}
