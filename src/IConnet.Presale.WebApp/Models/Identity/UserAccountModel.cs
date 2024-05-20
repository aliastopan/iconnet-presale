using IConnet.Presale.Shared.Contracts.Identity;

namespace IConnet.Presale.WebApp.Models.Identity;

public class UserAccountModel
{
    public UserAccountModel(UserAccountDto userAccountDto)
    {
        UserAccountId = userAccountDto.UserAccountId;
        Username = userAccountDto.Username;
        UserRole = EnumProcessor.StringToEnum<UserRole>(userAccountDto.UserRole);
        JobTitle = userAccountDto.JobTitle;
        LastLoggedIn = userAccountDto.LastLoggedIn;
    }

    public Guid UserAccountId { get; init; }
    public string Username { get; init; }
    public UserRole UserRole { get; init; }
    public string JobTitle { get; init; }
    public DateTime LastLoggedIn { get; init; }
}
