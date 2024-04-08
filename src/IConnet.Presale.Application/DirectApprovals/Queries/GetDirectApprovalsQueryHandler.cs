using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.DirectApprovals.Queries;

public class GetDirectApprovalsQueryHandler
    : IRequestHandler<GetDirectApprovalsQuery, Result<GetDirectApprovalsQueryResponse>>
{
    private readonly IDirectApprovalHandler _directApprovalHandler;

    public GetDirectApprovalsQueryHandler(IDirectApprovalHandler directApprovalHandler)
    {
        _directApprovalHandler = directApprovalHandler;
    }

    public ValueTask<Result<GetDirectApprovalsQueryResponse>> Handle(GetDirectApprovalsQuery request,
        CancellationToken cancellationToken)
    {
        Result<GetDirectApprovalsQueryResponse> result;

        // direct approval
        var tryGetDirectApprovals = _directApprovalHandler.TryGetDirectApprovals();
        if (tryGetDirectApprovals.IsFailure())
        {
            result = Result<GetDirectApprovalsQueryResponse>.Inherit(result: tryGetDirectApprovals);
            return ValueTask.FromResult(result);
        }

        var directApprovals = tryGetDirectApprovals.Value;

        var directApprovalDtos = new List<DirectApprovalDto>();
        foreach (var directApproval in directApprovals)
        {
            directApprovalDtos.Add(new DirectApprovalDto
            {
                DirectApprovalId = directApproval.DirectApprovalId,
                Order = directApproval.Order,
                Description = directApproval.Description,
                IsDeleted = directApproval.IsDeleted
            });
        }

        var response = new GetDirectApprovalsQueryResponse(directApprovalDtos);
        result = Result<GetDirectApprovalsQueryResponse>.Ok(response);

        return ValueTask.FromResult(result);
    }
}
