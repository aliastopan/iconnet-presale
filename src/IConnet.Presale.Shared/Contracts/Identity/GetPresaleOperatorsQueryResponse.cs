#nullable disable

namespace IConnet.Presale.Shared.Contracts.Identity;

public record GetPresaleOperatorsQueryResponse(List<PresaleOperatorDto> PresaleOperatorDtos);

public record PresaleOperatorDto
{
    public Guid UserAccountId { get; init; }
    public string Username { get; init; }
    public string UserRole { get; init; }
}