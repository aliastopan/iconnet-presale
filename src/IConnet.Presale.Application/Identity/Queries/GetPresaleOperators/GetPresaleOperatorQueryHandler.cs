using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Shared.Contracts.Identity;

namespace IConnet.Presale.Application.Identity.Queries.GetPresaleOperators;

public class GetPresaleOperatorQueryHandler : IRequestHandler<GetPresaleOperatorQuery, Result<GetPresaleOperatorsQueryResponse>>
{
    private readonly IIdentityAggregateHandler _identityAggregateHandler;

    public GetPresaleOperatorQueryHandler(IIdentityAggregateHandler identityAggregateHandler)
    {
        _identityAggregateHandler = identityAggregateHandler;
    }

    public async ValueTask<Result<GetPresaleOperatorsQueryResponse>> Handle(GetPresaleOperatorQuery request,
        CancellationToken cancellationToken)
    {
        Result<List<UserAccount>> tryGetPresaleOperators = await _identityAggregateHandler.TryGetRangePresaleOperatorAsync();

        if (tryGetPresaleOperators.IsFailure())
        {
            return Result<GetPresaleOperatorsQueryResponse>.Inherit(result: tryGetPresaleOperators);
        }

        List<UserAccount> userAccounts = tryGetPresaleOperators.Value;
        List<PresaleOperatorDto> presaleOperatorDtos = [];

        foreach (var userAccount in userAccounts)
        {
            presaleOperatorDtos.Add(new PresaleOperatorDto
            {
                UserAccountId = userAccount.UserAccountId,
                Username = userAccount.User.Username,
                UserRole = userAccount.User.UserRole.ToString()
            });
        }

        GetPresaleOperatorsQueryResponse response = new(presaleOperatorDtos);

        return Result<GetPresaleOperatorsQueryResponse>.Ok(response);
    }
}
