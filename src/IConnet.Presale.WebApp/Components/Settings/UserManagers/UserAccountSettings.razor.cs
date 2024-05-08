using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Settings.UserManagers;

public partial class UserAccountSettings : ComponentBase
{
    [Inject] protected UserManager UserManager { get; set; } = default!;

    [Parameter]
    public IQueryable<UserAccountModel>? UserAccounts { get; set; }

    protected GridSort<UserAccountModel> SortByUsername => GridSort<UserAccountModel>.ByAscending(user => user.Username);
    protected GridSort<UserAccountModel> SortByUserRole => GridSort<UserAccountModel>.ByAscending(user => user.UserRole);

    protected string GridTemplateCols => GetGridTemplateCols();

    protected override void OnInitialized()
    {
        if (UserAccounts is null)
        {
            return;
        }

        Log.Information("Credentials {0}", UserAccounts.Count());
    }

    protected string GetWidthStyle(int widthPx, int offsetPx = 0)
    {
        return $"width: {widthPx + offsetPx}px;";
    }

    private static string GetGridTemplateCols()
    {
        return $"{250}px {150}px {300}px;";
    }
}
