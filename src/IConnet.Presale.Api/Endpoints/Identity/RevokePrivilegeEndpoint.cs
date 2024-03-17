using IConnet.Presale.Application.Identity.Commands.RevokeUserPrivilege;
using IConnet.Presale.Shared.Contracts.Identity.UserPrivilege;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class RevokePrivilegeEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(UriEndpoint.Identity.RevokePrivilege, RevokePrivilege)
            .RequireAuthorization(Policies.AdministratorPrivilege);
    }

    internal async Task<IResult> RevokePrivilege([FromServices] ISender sender,
        GrantPrivilegeRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(new RevokePrivilegeCommand(request.AuthorityAccountId,
            request.AccessPassword,
            request.SubjectAccountId,
            request.Privilege));

        return result.Match(() => Results.Ok(),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to Grant Privilege"
            },
            context: httpContext));
    }
}
