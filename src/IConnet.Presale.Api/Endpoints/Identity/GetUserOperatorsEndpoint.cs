using IConnet.Presale.Application.Identity.Queries.GetUserOperators;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class GetUserOperatorsEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.Identity.GetOperators, GetUserOperators).AllowAnonymous();
    }

    internal async Task<IResult> GetUserOperators([FromServices] ISender sender,
        HttpContext httpContext)
    {
        var request = new GetUserOperatorQuery();
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
