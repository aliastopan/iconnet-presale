using System.Linq.Expressions;

namespace IConnet.Presale.WebApp.Helpers;

public class ImportModelColumnWidth : ColumnWidthBase
{
    public void SetColumnWidth(IQueryable<IApprovalOpportunityModel>? importModels)
    {
        if (importModels is null || !importModels.Any())
        {
            return;
        }

        SetColumnWidth<int>(importModels, crm => crm.NamaPemohon.Length, width => NamaPemohonPx = width, "Nama Pemohon");
        SetColumnWidth<int>(importModels, crm => crm.EmailPemohon.Length, width => EmailPemohonPx = width, "Email Pemohon");
        SetColumnWidth<int>(importModels, crm => crm.AlamatPemohon.Length, width => AlamatPemohonPx = width, "Alamat Pemohon");
        SetColumnWidth<int>(importModels, crm => crm.NamaAgen.Length, width => NamaAgenPx = width, "Nama Agen");
        SetColumnWidth<int>(importModels, crm => crm.EmailAgen.Length, width => EmailAgenPx = width, "Email Agen");
        SetColumnWidth<int>(importModels, crm => crm.MitraAgen.Length, width => MitraAgenPx = width, "Mitra Agen");
        SetColumnWidth<int>(importModels, crm => crm.Keterangan.Length, width => KeteranganPx = width, "Keterangan");
    }

    private void SetColumnWidth<T>(IQueryable<IApprovalOpportunityModel> importModels, Expression<Func<IApprovalOpportunityModel, int>>
        propertySelector, Action<int> setProperty, string propertyName)
    {
        if (importModels is null || !importModels.Any())
        {
            return;
        }

        int contentWidth = importModels.Max(propertySelector.Compile());
        int columnWidthPx = (contentWidth * CharWidth) + Padding;
        Log.Warning("{0} length: {1}, width: {2}px", propertyName, contentWidth, columnWidthPx);

        if (columnWidthPx > DefaultWidth)
        {
            setProperty(columnWidthPx);
        }
    }
}
