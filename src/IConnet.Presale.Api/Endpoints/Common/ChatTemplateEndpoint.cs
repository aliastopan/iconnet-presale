using IConnet.Presale.Application.ChatTemplates.Commands.ChatTemplateAction;
using IConnet.Presale.Application.ChatTemplates.Queries.GetChatTemplates;
using IConnet.Presale.Application.ChatTemplates.Queries.GetAvailableChatTemplates;
using IConnet.Presale.Shared.Contracts.Common;


namespace IConnet.Presale.Api.Endpoints.Common;

public class ChatTemplateEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.ChatTemplate.GetChatTemplates, GetChatTemplates).AllowAnonymous();
        app.MapGet(UriEndpoint.ChatTemplate.GetAvailableChatTemplates, GetAvailableChatTemplates).AllowAnonymous();
        app.MapGet(UriEndpoint.ChatTemplate.ChatTemplateAction, ChatTemplateAction).AllowAnonymous();
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


    internal async Task<IResult> GetAvailableChatTemplates([FromServices] ISender sender,
        HttpContext httpContext)
    {
        var query = new GetAvailableChatTemplatesQuery();
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

    internal async Task<IResult> ChatTemplateAction([FromServices] ISender sender,
        ChatTemplateActionRequest request, HttpContext httpContext)
    {
        var result = await sender.Send(new ChatTemplateActionCommand(
            request.ChatTemplateId,
            request.TemplateName,
            request.Sequence,
            request.Content,
            request.Action));

        return result.Match(() => Results.Ok(),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to perform Chat Template action"
            },
            context: httpContext));
    }
}
