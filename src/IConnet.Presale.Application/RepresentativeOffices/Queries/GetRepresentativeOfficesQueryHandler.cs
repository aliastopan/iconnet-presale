using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.RepresentativeOffices.Queries;

public class GetRepresentativeOfficesQueryHandler
    : IRequestHandler<GetRepresentativeOfficesQuery, Result<GetRepresentativeOfficesQueryResponse>>
{
    private readonly IRepresentativeOfficeHandler _representativeOfficeHandler;

    public GetRepresentativeOfficesQueryHandler(IRepresentativeOfficeHandler representativeOfficeHandler)
    {
        _representativeOfficeHandler = representativeOfficeHandler;
    }

    public ValueTask<Result<GetRepresentativeOfficesQueryResponse>> Handle(GetRepresentativeOfficesQuery request,
        CancellationToken cancellationToken)
    {
        Result<GetRepresentativeOfficesQueryResponse> result;

        // representative office
        var tryGetRepresentativeOffices = _representativeOfficeHandler.TryGetRepresentativesOffices();
        if (tryGetRepresentativeOffices.IsFailure())
        {
            result = Result<GetRepresentativeOfficesQueryResponse>.Inherit(result: tryGetRepresentativeOffices);
            return ValueTask.FromResult(result);
        }

        var representativeOffices = tryGetRepresentativeOffices.Value;

        var representativeOfficeDtos = new List<RepresentativeOfficeDto>();
        foreach (var representativeOffice in representativeOffices)
        {
            representativeOfficeDtos.Add(new RepresentativeOfficeDto
            {
                Order = representativeOffice.Order,
                Perwakilan = representativeOffice.Perwakilan,
                IsDeleted = representativeOffice.IsDeleted
            });
        }

        var response = new GetRepresentativeOfficesQueryResponse(representativeOfficeDtos);
        result = Result<GetRepresentativeOfficesQueryResponse>.Ok(response);

        return ValueTask.FromResult(result);
    }
}
