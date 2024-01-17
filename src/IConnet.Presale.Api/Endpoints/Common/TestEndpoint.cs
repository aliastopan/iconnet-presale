using IConnet.Presale.Application.Common.Interfaces.Services;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Api.Endpoints.Common;

public class TestEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("/api/send-email", SendEmail).AllowAnonymous();
    }

    internal async Task<IResult> SendEmail([FromServices] IMailService mailService,
        SendEmailRequest request, HttpContext httpContext)
    {
        await mailService.Send
        (
            request.EmailAddressTo,
            request.EmailAddressFrom,
            request.Password,
            request.Subject,
            request.Body,
            request.SmtpHost,
            request.Port
        );

        return Results.Ok();
    }
}
