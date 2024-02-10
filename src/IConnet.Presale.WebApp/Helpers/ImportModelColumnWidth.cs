namespace IConnet.Presale.WebApp.Helpers;

public class ImportModelColumnWidth : ColumnWidthBase<IApprovalOpportunityModel>
{
    public override void SetColumnWidth(IQueryable<IApprovalOpportunityModel>? importModels)
    {
        if (importModels is null || !importModels.Any())
        {
            return;
        }

        SetColumnWidth(importModels, crm => crm.NamaPemohon.Length, width => NamaPemohonPx = width, "Nama Pemohon");
        SetColumnWidth(importModels, crm => crm.EmailPemohon.Length, width => EmailPemohonPx = width, "Email Pemohon");
        SetColumnWidth(importModels, crm => crm.AlamatPemohon.Length, width => AlamatPemohonPx = width, "Alamat Pemohon");
        SetColumnWidth(importModels, crm => crm.NamaAgen.Length, width => NamaAgenPx = width, "Nama Agen");
        SetColumnWidth(importModels, crm => crm.EmailAgen.Length, width => EmailAgenPx = width, "Email Agen");
        SetColumnWidth(importModels, crm => crm.MitraAgen.Length, width => MitraAgenPx = width, "Mitra Agen");
        SetColumnWidth(importModels, crm => crm.Keterangan.Length, width => KeteranganPx = width, "Keterangan");
    }
}
