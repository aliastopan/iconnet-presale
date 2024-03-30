#nullable disable

namespace IConnet.Presale.Shared.Contracts.Common;

public record GetDirectApprovalsQueryResponse(ICollection<DirectApprovalDto> DirectApprovalDto);

public record DirectApprovalDto
{
    public int Order { get; set; }
    public string Description { get; init; }
    public bool IsDeleted { get; init; }
}
