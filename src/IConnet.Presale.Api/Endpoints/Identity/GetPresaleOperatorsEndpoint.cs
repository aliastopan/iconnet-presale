using IConnet.Presale.Application.Identity.Queries.GetPresaleOperators;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class GetPresaleOperatorsEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.Identity.GetPresaleOperators, GetPresaleOperators).AllowAnonymous();
    }

    internal async Task<IResult> GetPresaleOperators([FromServices] ISender sender,
        HttpContext httpContext)
    {
        var request = new GetPresaleOperatorQuery();
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
