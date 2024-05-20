using IConnet.Presale.Application.Identity.Queries.GetUserAccounts;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class GetUserAccountsEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.Identity.GetUserAccounts, GetUserAccounts).AllowAnonymous();
    }

    internal async Task<IResult> GetUserAccounts([FromServices] ISender sender,
        HttpContext httpContext)
    {
        var request = new GetUserAccountsQuery();
        var result = await sender.Send(request);

        return result.Match(
            value => Results.Ok(value),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Not Found"
            },
            context: httpContext));
    }
}

