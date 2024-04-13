using IConnet.Presale.Application.DirectApprovals.Commands.AddDirectApproval;
using IConnet.Presale.Application.DirectApprovals.Commands.ToggleSoftDeletion;
using IConnet.Presale.Application.DirectApprovals.Queries;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Api.Endpoints.Common;

public class DirectApprovalEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.DirectApproval.GetDirectApprovals, GetDirectApprovals).AllowAnonymous();
        app.MapPost(UriEndpoint.DirectApproval.AddDirectApproval, AddDirectApproval).AllowAnonymous();
        app.MapPost(UriEndpoint.DirectApproval.ToggleSoftDeletion, ToggleSoftDeletion).AllowAnonymous();
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

    internal async Task<IResult> AddDirectApproval([FromServices] ISender sender,
        AddDirectApprovalRequest request, HttpContext httpContext)
    {
        var command = new AddDirectApprovalCommand(request.Order, request.Description);
        var result = await sender.Send(command);

        return result.Match(() => Results.Ok(),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to add Direct Approval"
            },
            context: httpContext));
    }

    internal async Task<IResult> ToggleSoftDeletion([FromServices] ISender sender,
        ToggleDirectApprovalSoftDeletionRequest request, HttpContext httpContext)
    {
        var command = new ToggleDirectApprovalSoftDeletionCommand(request.DirectApprovalId, request.IsDeleted);
        var result = await sender.Send(command);

        return result.Match(() => Results.Ok(),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to Toggle Soft Deletion"
            },
            context: httpContext));
    }
}
