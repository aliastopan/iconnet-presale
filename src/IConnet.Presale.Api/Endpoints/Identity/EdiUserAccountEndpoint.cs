using IConnet.Presale.Application.Identity.Commands.EditUserAccount;
using IConnet.Presale.Shared.Contracts.Identity.EditUserAccount;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class EdiUserAccountEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(UriEndpoint.Identity.EditUserAccount, EditUserAccount).AllowAnonymous();
    }

    internal async Task<IResult> EditUserAccount([FromServices] ISender sender,
        EditUserAccountRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(new EditUserAccountCommand(request.UserAccountId,
            request.NewUsername,
            request.NewPassword,
            request.ConfirmPassword,
            request.IsChangeUsername,
            request.IsChangePassword));

        return result.Match(() =>
            {
                httpContext.Response.Cookies.Delete("access-token");
                httpContext.Response.Cookies.Delete("refresh-token");

                return Results.Ok();
            },
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to Edit User Account"
            },
            context: httpContext));
    }
}
