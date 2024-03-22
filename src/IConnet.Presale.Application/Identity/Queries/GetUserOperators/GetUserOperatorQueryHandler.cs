using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Shared.Contracts.Identity;

namespace IConnet.Presale.Application.Identity.Queries.GetUserOperators;

public class GetUserOperatorQueryHandler : IRequestHandler<GetUserOperatorQuery, Result<GetUserOperatorsQueryResponse>>
{
    private readonly IIdentityAggregateHandler _identityAggregateHandler;

    public GetUserOperatorQueryHandler(IIdentityAggregateHandler identityAggregateHandler)
    {
        _identityAggregateHandler = identityAggregateHandler;
    }

    public async ValueTask<Result<GetUserOperatorsQueryResponse>> Handle(GetUserOperatorQuery request,
        CancellationToken cancellationToken)
    {
        Result<List<UserAccount>> tryGetUserOperators = await _identityAggregateHandler.TryGetRangeUserOperatorAsync();

        if (tryGetUserOperators.IsFailure())
        {
            return Result<GetUserOperatorsQueryResponse>.Inherit(result: tryGetUserOperators);
        }

        List<UserAccount> userAccounts = tryGetUserOperators.Value;
        List<UserOperatorDto> userOperatorDtos = [];

        foreach (var userAccount in userAccounts)
        {
            userOperatorDtos.Add(new UserOperatorDto
            {
                UserAccountId = userAccount.UserAccountId,
                Username = userAccount.User.Username,
                UserRole = userAccount.User.UserRole.ToString()
            });
        }

        GetUserOperatorsQueryResponse response = new(userOperatorDtos);

        return Result<GetUserOperatorsQueryResponse>.Ok(response);
    }
}
