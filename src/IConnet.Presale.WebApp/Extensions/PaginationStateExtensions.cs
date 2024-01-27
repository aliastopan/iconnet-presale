using Microsoft.FluentUI.AspNetCore.Components;

namespace IConnet.Presale.WebApp.Extensions;

public static class PaginationStateExtensions
{
    public static void DynamicPagePerItem(this PaginationState paginationState, int count, int itemPerPage)
    {
        paginationState.ItemsPerPage = count < itemPerPage ? count : itemPerPage;
    }
}
