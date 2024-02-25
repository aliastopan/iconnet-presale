using IConnet.Presale.Application.Identity.Commands.Registration;
using IConnet.Presale.Shared.Contracts.Identity.Registration;

namespace IConnet.Presale.Api.Endpoints.Identity;

public class RegistrationEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(RouteEndpoint.Identity.SignUp, SignUp).AllowAnonymous();
    }

    internal async Task<IResult> SignUp([FromServices] ISender sender,
        SignUpRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(new SignUpCommand(request.Username,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.EmailAddress,
            request.Password,
            request.EmploymentStatus,
            request.UserRole,
            request.JobTitle,
            request.JobShift)
        );

        return result.Match(
            value => Results.Ok(value),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to Register User"
            },
            context: httpContext));
    }
}
