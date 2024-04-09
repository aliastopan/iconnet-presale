using IConnet.Presale.Application.RootCauses.Commands;
using IConnet.Presale.Application.RootCauses.Queries;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Api.Endpoints.Common;

public class RootCauseEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.RootCauses.GetRootCauses, GetRootCauses).AllowAnonymous();
        app.MapPost(UriEndpoint.RootCauses.AddRootCauses, AddRootCause).AllowAnonymous();
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

    internal async Task<IResult> AddRootCause([FromServices] ISender sender,
        AddRootCauseRequest request, HttpContext httpContext)
    {
        var command = new AddRootCauseCommand(request.Order, request.Cause);
        var result = await sender.Send(command);

        return result.Match(() => Results.Ok(),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to add Root Cause"
            },
            context: httpContext));
    }
}
