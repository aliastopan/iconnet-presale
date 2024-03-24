using IConnet.Presale.Shared.Contracts.Identity;

namespace IConnet.Presale.WebApp.Models.Identity;

public record PresaleOperatorModel
{
    public PresaleOperatorModel(PresaleOperatorDto presaleOperatorDto)
    {
        UserAccountId = presaleOperatorDto.UserAccountId;
        Username = presaleOperatorDto.Username;
        UserRole = EnumProcessor.StringToEnum<UserRole>(presaleOperatorDto.UserRole);
    }

    public Guid UserAccountId { get; init; }
    public string Username { get; init; }
    public UserRole UserRole { get; init; }
}
