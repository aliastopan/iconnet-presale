using IConnet.Presale.Application.Identity.Commands.GrantUserPrivilege;
using IConnet.Presale.Shared.Contracts.Identity.UserPrivilege;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class GrantPrivilegeEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(UriEndpoint.Identity.GrantPrivilege, GrantPrivilege)
            .RequireAuthorization(Policies.AdministratorPrivilege);
    }

    internal async Task<IResult> GrantPrivilege([FromServices] ISender sender,
        GrantPrivilegeRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(new GrantPrivilegeCommand(request.AuthorityAccountId,
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
