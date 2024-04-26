using IConnet.Presale.Application.RootCauses.Commands.AddRootCause;
using IConnet.Presale.Application.RootCauses.Commands.ToggleOptions;
using IConnet.Presale.Application.RootCauses.Queries;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Api.Endpoints.Common;

public class RootCauseEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.RootCauses.GetRootCauses, GetRootCauses).AllowAnonymous();
        app.MapPost(UriEndpoint.RootCauses.AddRootCause, AddRootCause).AllowAnonymous();
        app.MapPost(UriEndpoint.RootCauses.ToggleOptions, ToggleOptions).AllowAnonymous();
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
        var command = new AddRootCauseCommand(request.Order, request.Cause, request.Classification);
        var result = await sender.Send(command);

        return result.Match(() => Results.Ok(),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to add Root Cause"
            },
            context: httpContext));
    }

    internal async Task<IResult> ToggleOptions([FromServices] ISender sender,
        ToggleRootCauseOptionsRequest request, HttpContext httpContext)
    {
        var command = new ToggleRootCauseOptionsCommand(request.RootCauseId, request.IsDeleted, request.IsOnVerification);
        var result = await sender.Send(command);

        return result.Match(() => Results.Ok(),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to Toggle Soft Deletion"
            },
            context: httpContext));
    }
}
