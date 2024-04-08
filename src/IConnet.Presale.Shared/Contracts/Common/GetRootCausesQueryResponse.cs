#nullable disable

namespace IConnet.Presale.Shared.Contracts.Common;

public record GetRootCausesQueryResponse(ICollection<RootCausesDto> RootCausesDtos);

public record RootCausesDto
{
    public Guid RootCauseId { get; init; }
    public int Order { get; init; }
    public string Cause { get; init; }
    public bool IsDeleted { get; init; }
}