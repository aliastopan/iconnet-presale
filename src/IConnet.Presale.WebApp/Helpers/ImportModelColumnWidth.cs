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

        SetColumnWidth(importModels, crm => crm.NamaPemohon, width => NamaPemohonPx = width);
        SetColumnWidth(importModels, crm => crm.EmailPemohon, width => EmailPemohonPx = width);
        SetColumnWidth(importModels, crm => crm.AlamatPemohon, width => AlamatPemohonPx = width);
        SetColumnWidth(importModels, crm => crm.NamaAgen, width => NamaAgenPx = width);
        SetColumnWidth(importModels, crm => crm.EmailAgen, width => EmailAgenPx = width);
        SetColumnWidth(importModels, crm => crm.MitraAgen, width => MitraAgenPx = width);
    }

    private void SetColumnWidth<T>(IQueryable<IApprovalOpportunityModel> importModels, Expression<Func<IApprovalOpportunityModel, T>>
        propertySelector, Action<int> setProperty)
    {
        if (importModels is null || !importModels.Any())
        {
            return;
        }

        var maxContentLength = importModels.Max(propertySelector.Compile());
        var columnWidth = ((maxContentLength?.ToString()!.Length ?? 0) * CharWidth) + Padding;
        setProperty(columnWidth);

        var propertyName = GetPropertyName(propertySelector);
        Log.Warning("{0}: {1}px", propertyName, columnWidth);
    }

    private string GetPropertyName<T>(Expression<Func<IApprovalOpportunityModel, T>> propertySelector)
    {
        MemberExpression memberExpression = (MemberExpression)propertySelector.Body;
        return memberExpression.Member.Name;
    }
}
