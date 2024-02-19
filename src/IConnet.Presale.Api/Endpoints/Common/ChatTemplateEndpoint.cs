using IConnet.Presale.Application.ChatTemplates.Queries;

namespace IConnet.Presale.Api.Endpoints.Common;

public class ChatTemplateEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(ApiEndpoint.ChatTemplate.GetChatTemplate, GetChatTemplates).AllowAnonymous();
    }

    internal async Task<IResult> GetChatTemplates([FromServices] ISender sender,
        [FromRoute] string templateName, HttpContext httpContext)
    {
        // LogSwitch.Debug("Request {0}", templateName);
        var query = new GetChatTemplateQuery(templateName);
        var result = await sender.Send(query);

        return result.Match(
            value =>
            {
                return Results.Ok(value);
            },
            fault => fault.AsProblem(new ProblemDetails
            {
                Type = "https://localhost:7244/errors/chat-template",
                Title = "Chat Template Query Failed",
                Instance = httpContext.Request.Path
            },
            context: httpContext));;
    }
}
