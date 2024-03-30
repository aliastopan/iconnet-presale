using IConnet.Presale.Application.DirectApprovals.Queries;

namespace IConnet.Presale.Api.Endpoints.Common;

public class DirectApprovalEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.DirectApproval.GetDirectApprovals, GetDirectApprovals).AllowAnonymous();
    }

    internal async Task<IResult> GetDirectApprovals([FromServices] ISender sender,
        HttpContext httpContext)
    {
        var query = new GetDirectApprovalsQuery();
        var result = await sender.Send(query);

        return result.Match(
            value =>
            {
                return Results.Ok(value);
            },
            fault => fault.AsProblem(new ProblemDetails
            {
                Title = "Direct Approvals Query Failed",
                Instance = httpContext.Request.Path
            },
            context: httpContext));;
    }
}
