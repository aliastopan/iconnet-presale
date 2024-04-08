#nullable disable

namespace IConnet.Presale.Shared.Contracts.Common;

public record GetDirectApprovalsQueryResponse(ICollection<DirectApprovalDto> DirectApprovalDto);

public record DirectApprovalDto
{
    public Guid DirectApprovalId { get; init; }
    public int Order { get; init; }
    public string Description { get; init; }
    public bool IsDeleted { get; init; }
}
