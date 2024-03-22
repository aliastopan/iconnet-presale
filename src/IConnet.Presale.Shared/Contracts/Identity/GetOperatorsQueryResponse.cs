#nullable disable

namespace IConnet.Presale.Shared.Contracts.Identity;

public record GetUserOperatorsQueryResponse(List<UserOperatorDto> UserOperatorDtos);

public record UserOperatorDto
{
    public Guid UserAccountId { get; init; }
    public string Username { get; init; }
    public string UserRole { get; init; }
}