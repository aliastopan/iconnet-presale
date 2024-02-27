#nullable disable

namespace IConnet.Presale.Shared.Contracts.Common;

public record GetRootCausesQueryResponse(ICollection<RootCausesDto> RootCausesDtos);

public record RootCausesDto
{
    public int Order { get; set; }
    public string Cause { get; init; }
    public bool IsDeleted { get; init; }
}