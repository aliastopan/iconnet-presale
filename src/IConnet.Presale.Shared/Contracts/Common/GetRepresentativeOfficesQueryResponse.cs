#nullable disable

namespace IConnet.Presale.Shared.Contracts.Common;

public record GetRepresentativeOfficesQueryResponse(ICollection<RepresentativeOfficeDto> RepresentativeOfficeDtos);

public record RepresentativeOfficeDto
{
    public int Order { get; set; }
    public string Perwakilan { get; init; }
    public bool IsDeleted { get; init; }
}
