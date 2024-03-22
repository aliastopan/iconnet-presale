using IConnet.Presale.Shared.Contracts.Identity;

namespace IConnet.Presale.WebApp.Models.Identity;

public record UserOperatorModel
{
    public UserOperatorModel(UserOperatorDto userOperatorDto)
    {
        UserAccountId = userOperatorDto.UserAccountId;
        Username = userOperatorDto.Username;
        UserRole = EnumProcessor.StringToEnum<UserRole>(userOperatorDto.UserRole);
    }

    public Guid UserAccountId { get; init; }
    public string Username { get; init; }
    public UserRole UserRole { get; init; }
}
