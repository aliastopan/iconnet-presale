#nullable disable

namespace IConnet.Presale.Shared.Contracts.Common;

public record GetRepresentativeOfficesQueryResponse(ICollection<RepresentativeOfficeDto> RepresentativeOfficeDtos);

public record RepresentativeOfficeDto
{
    public Guid RepresentativeOfficeId { get; init; }
    public int Order { get; init; }
    public string Perwakilan { get; init; }
    public bool IsDeleted { get; init; }
}
