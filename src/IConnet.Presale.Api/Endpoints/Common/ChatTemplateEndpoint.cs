using IConnet.Presale.Application.ChatTemplates.Queries.GetChatTemplates;

namespace IConnet.Presale.Api.Endpoints.Common;

public class ChatTemplateEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.ChatTemplate.GetChatTemplates, GetChatTemplates).AllowAnonymous();
    }

    internal async Task<IResult> GetChatTemplates([FromServices] ISender sender,
        [FromRoute] string templateName, HttpContext httpContext)
    {
        var query = new GetChatTemplatesQuery(templateName);
        var result = await sender.Send(query);

        return result.Match(
            value =>
            {
                return Results.Ok(value);
            },
            fault => fault.AsProblem(new ProblemDetails
            {
                Title = "Chat Templates Query Failed",
                Instance = httpContext.Request.Path
            },
            context: httpContext));;
    }
}
