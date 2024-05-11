using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Settings.UserManagers;

public partial class UserAccountSettings : ComponentBase
{
    [Inject] protected UserManager UserManager { get; set; } = default!;
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected IToastService ToastService { get; set; } = default!;

    private List<UserAccountModel> _userAccounts { get; set; } = [];
    protected IQueryable<UserAccountModel>? UserAccounts => _userAccounts
        .Where(user => user.UserRole < UserRole.SuperUser)
        .OrderBy(user => user.Username)
        .AsQueryable();

    protected GridSort<UserAccountModel> SortByUsername => GridSort<UserAccountModel>.ByAscending(user => user.Username);
    protected GridSort<UserAccountModel> SortByUserRole => GridSort<UserAccountModel>.ByAscending(user => user.UserRole);

    public bool IsInitialized { get; set; } = true;
    public bool IsLoading { get; set; } = false;
    protected string GridTemplateCols => GetGridTemplateCols();

    protected override void OnInitialized()
    {
        if (UserAccounts is null)
        {
            return;
        }

        Log.Information("Credentials {0}", UserAccounts.Count());
    }

    protected override async Task OnInitializedAsync()
    {
        IsInitialized = false;

        _userAccounts = await UserManager.GetUserAccountsAsync();

        IsInitialized = true;
    }

    protected async Task OpenChangePasswordDialogAsync(UserAccountModel userAccount)
    {
        LogSwitch.Debug("Edit: {0}", userAccount.Username);

        await Task.CompletedTask;

        var parameters = new DialogParameters()
        {
            Title = "Ganti Password",
            TrapFocus = true,
            Width = "600px",
        };

        var target = new EditUserAccountModel(userAccount.UserAccountId);

        var dialog = await DialogService.ShowDialogAsync<ChangePasswordDialog>(target, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (EditUserAccountModel)result.Data;

        IsLoading = true;
        this.StateHasChanged();

        bool isSuccessStatusCode = await UserManager.ChangePasswordAsync(
            dialogData.UserAccountId,
            dialogData.NewPassword,
            dialogData.ConfirmPassword);

        ChangePasswordToast(isSuccessStatusCode, userAccount.Username);

        IsLoading = false;
        this.StateHasChanged();
    }

    protected async Task OpenChangeUsernameDialogAsync(UserAccountModel userAccount)
    {
        LogSwitch.Debug("Edit: {0}", userAccount.Username);

        await Task.CompletedTask;

        var parameters = new DialogParameters()
        {
            Title = $"Ganti Username ({userAccount.Username})",
            TrapFocus = true,
            Width = "600px",
        };

        var target = new EditUserAccountModel(userAccount.UserAccountId);

        var dialog = await DialogService.ShowDialogAsync<ChangeUsernameDialog>(target, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (EditUserAccountModel)result.Data;

        bool isDuplicateUsername = UserAccounts!
            .Any(userAccount => string.Equals(userAccount.Username, dialogData.NewUsername, StringComparison.OrdinalIgnoreCase));

        if (isDuplicateUsername)
        {
            LogSwitch.Debug("Duplicate Username: {0}", userAccount.Username);

            return;
        }

        IsLoading = true;
        this.StateHasChanged();

        bool isSuccessStatusCode = await UserManager.ChangeUsernameAsync(
            dialogData.UserAccountId,
            dialogData.NewUsername);

        _userAccounts = await UserManager.GetUserAccountsAsync();
        await UserManager.SetPresaleOperatorsAsync();

        ChangeUsernameToast(isSuccessStatusCode, userAccount.Username, dialogData.NewUsername);

        IsLoading = false;
        this.StateHasChanged();
    }

    private void ChangePasswordToast(bool isSuccess, string username)
    {
        if (isSuccess)
        {
            var intent = ToastIntent.Success;
            var message = $"Password untuk User '{username}' telah berhasil diganti";
            var timeout = 5000; // milliseconds

            ToastService.ShowToast(intent, message, timeout: timeout);
        }
        else
        {
            var intent = ToastIntent.Error;
            var message = $"Terjadi kesalahan. Gagal mengganti password untuk User '{username}'";
            var timeout = 5000; // milliseconds

            ToastService.ShowToast(intent, message, timeout: timeout);
        }
    }

    private void ChangeUsernameToast(bool isSuccess, string username, string newUsername)
    {
        if (isSuccess)
        {
            var intent = ToastIntent.Success;
            var message = $"Username '{username}' telah berhasil diganti menjadi `{newUsername}`";
            var timeout = 8000; // milliseconds

            ToastService.ShowToast(intent, message, timeout: timeout);
        }
        else
        {
            var intent = ToastIntent.Error;
            var message = $"Terjadi kesalahan. Gagal mengganti username untuk User '{username}'";
            var timeout = 5000; // milliseconds

            ToastService.ShowToast(intent, message, timeout: timeout);
        }
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
