#nullable disable

namespace IConnet.Presale.Shared.Contracts.Identity;

public record GetUserAccountsQueryResponse(List<UserAccountDto> UserAccountDtos);

public record UserAccountDto
{
    public Guid UserAccountId { get; init; }
    public string Username { get; init; }
    public string UserRole { get; init; }
    public string JobTitle { get; init; }
    public List<string> UserPrivileges { get; init; }
    public DateTime LastLoggedIn { get; init; }
}