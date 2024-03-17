using IConnet.Presale.Application.Identity.Commands.SetUserRole;
using IConnet.Presale.Shared.Contracts.Identity.SetUserRole;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class SetRoleEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(UriEndpoint.Identity.SetRole, SetRole)
            .RequireAuthorization(Policies.AdministratorPrivilege);
    }

    internal async Task<IResult> SetRole([FromServices] ISender sender,
        SetRoleRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(new SetUserRoleCommand(request.AuthorityAccountId,
            request.AccessPassword,
            request.SubjectAccountId,
            request.Role));

        return result.Match(() => Results.Ok(),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to Update User Role"
            },
            context: httpContext));
    }
}
