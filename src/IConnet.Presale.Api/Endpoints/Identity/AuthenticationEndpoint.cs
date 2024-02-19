using IConnet.Presale.Application.Identity.Commands.Authentication;
using IConnet.Presale.Shared.Contracts.Identity.Authentication;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class AuthenticationEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(RouteEndpoint.Identity.SignIn, SignIn).AllowAnonymous();
        app.MapPost(RouteEndpoint.Identity.SignOut, SignOut).AllowAnonymous();
    }

    internal async Task<IResult> SignIn([FromServices] ISender sender,
        SignInRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(new SignInCommand(request.Username, request.Password));

        return result.Match(
            value =>
            {
                var cookieOption = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMonths(1)
                };
                httpContext.Response.Cookies.Append("access-token", value.AccessToken, cookieOption);
                httpContext.Response.Cookies.Append("refresh-token", value.RefreshTokenStr, cookieOption);
                return Results.Ok(value);
            },
            fault => fault.AsProblem(new ProblemDetails
            {
                Type = "https://localhost:7244/errors/authentication",
                Title = "Authentication Failed",
                Instance = httpContext.Request.Path
            },
            context: httpContext));
    }

    internal IResult SignOut(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete("access-token");
        httpContext.Response.Cookies.Delete("refresh-token");

        return Results.Ok();
    }
}
