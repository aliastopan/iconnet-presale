using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.RootCauses.Queries;

public class GetRootCausesQueryHandler
    : IRequestHandler<GetRootCausesQuery, Result<GetRootCausesQueryResponse>>
{
    private readonly IRootCauseManager _rootCauseManager;

    public GetRootCausesQueryHandler(IRootCauseManager rootCauseManager)
    {
        _rootCauseManager = rootCauseManager;
    }

    public ValueTask<Result<GetRootCausesQueryResponse>> Handle(GetRootCausesQuery request,
        CancellationToken cancellationToken)
    {
        Result<GetRootCausesQueryResponse> result;

        // root cause
        var tryGetRootCauses = _rootCauseManager.TryGetRootCauses();
        if (tryGetRootCauses.IsFailure())
        {
            result = Result<GetRootCausesQueryResponse>.Inherit(result: tryGetRootCauses);
            return ValueTask.FromResult(result);
        }

        var rootCauses = tryGetRootCauses.Value;

        var rootCauseDtos = new List<RootCausesDto>();
        foreach (var rootCause in rootCauses)
        {
            rootCauseDtos.Add(new RootCausesDto
            {
                Order = rootCause.Order,
                Cause = rootCause.Cause,
                IsDeleted = rootCause.IsDeleted
            });
        }

        var response = new GetRootCausesQueryResponse(rootCauseDtos);
        result = Result<GetRootCausesQueryResponse>.Ok(response);

        return ValueTask.FromResult(result);
    }
}
