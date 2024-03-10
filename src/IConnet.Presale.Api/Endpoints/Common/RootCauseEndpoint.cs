using IConnet.Presale.Application.RootCauses.Queries;

namespace IConnet.Presale.Api.Endpoints.Common;

public class RootCauseEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(RouteEndpoint.RootCauses.GetRootCauses, GetRootCauses).AllowAnonymous();
    }

    internal async Task<IResult> GetRootCauses([FromServices] ISender sender,
        HttpContext httpContext)
    {
        var query = new GetRootCausesQuery();
        var result = await sender.Send(query);

        return result.Match(
            value =>
            {
                return Results.Ok(value);
            },
            fault => fault.AsProblem(new ProblemDetails
            {
                Title = "Root Cause Query Failed",
                Instance = httpContext.Request.Path
            },
            context: httpContext));;
    }
}
