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

        SetColumnWidth<int>(importModels, crm => crm.NamaPemohon.Length, width => NamaPemohonPx = width);
        SetColumnWidth<int>(importModels, crm => crm.EmailPemohon.Length, width => EmailPemohonPx = width);
        SetColumnWidth<int>(importModels, crm => crm.AlamatPemohon.Length, width => AlamatPemohonPx = width);
        SetColumnWidth<int>(importModels, crm => crm.NamaAgen.Length, width => NamaAgenPx = width);
        SetColumnWidth<int>(importModels, crm => crm.EmailAgen.Length, width => EmailAgenPx = width);
        SetColumnWidth<int>(importModels, crm => crm.MitraAgen.Length, width => MitraAgenPx = width);
    }

    private void SetColumnWidth<T>(IQueryable<IApprovalOpportunityModel> importModels, Expression<Func<IApprovalOpportunityModel, int>>
        propertySelector, Action<int> setProperty)
    {
        if (importModels is null || !importModels.Any())
        {
            return;
        }

        int contentWidth = importModels.Max(propertySelector.Compile());
        int columnWidthPx = (contentWidth * CharWidth) + Padding;
        setProperty(columnWidthPx);

        Log.Warning("content width: {0}, column width: {1}px", contentWidth, columnWidthPx);
    }

    private string GetPropertyName<T>(Expression<Func<IApprovalOpportunityModel, T>> propertySelector)
    {
        MemberExpression memberExpression = (MemberExpression)propertySelector.Body;
        return memberExpression.Member.Name;
    }
}
