using IConnet.Presale.Application.Identity.Commands.ResetPassword;
using IConnet.Presale.Shared.Contracts.Identity.ResetPassword;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class ResetPasswordEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(ApiRoute.Identity.ResetPassword, ResetPassword);
    }

    internal async Task<IResult> ResetPassword([FromServices] ISender sender,
        ResetPasswordRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(new ResetPasswordCommand(
            request.UserAccountId,
            request.OldPassword,
            request.NewPassword,
            request.ConfirmPassword));

        return result.Match(() =>
            {
                httpContext.Response.Cookies.Delete("access-token");
                httpContext.Response.Cookies.Delete("refresh-token");

                return Results.Ok();
            },
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to Reset Password"
            },
            context: httpContext));
    }
}
