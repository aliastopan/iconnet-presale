#nullable disable

namespace IConnet.Presale.Shared.Contracts.Identity;

public record GetUserAccountsQueryResponse(List<UserAccountDto> UserAccountDtos);

public record UserAccountDto
{
    public Guid UserAccountId { get; set; }
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string UserRole { get; set; }
    public List<string> UserPrivileges { get; set; }
    public DateTime LastLoggedIn { get; set; }
}