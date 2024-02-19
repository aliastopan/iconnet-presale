using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.ChatTemplates.Queries;

public class GetChatTemplateQueryHandler : IRequestHandler<GetChatTemplateQuery, Result<GetChatTemplateResponse>>
{
    private readonly IChatTemplateManager _chatTemplateManager;

    public GetChatTemplateQueryHandler(IChatTemplateManager chatTemplateManager)
    {
        _chatTemplateManager = chatTemplateManager;
    }

    public async ValueTask<Result<GetChatTemplateResponse>> Handle(GetChatTemplateQuery request,
        CancellationToken cancellationToken)
    {
        // data annotation validations
        var isInvalid = !request.TryValidate(out var errors);
        if (isInvalid)
        {
            return Result<GetChatTemplateResponse>.Invalid(errors);
        }

        // chat template
        var tryGetChatTemplates = await _chatTemplateManager.TryGetChatTemplatesAsync(request.TemplateName);
        if (tryGetChatTemplates.IsFailure())
        {
            return Result<GetChatTemplateResponse>.Inherit(result: tryGetChatTemplates);
        }

        var chatTemplates = tryGetChatTemplates.Value;

        var chatTemplateDtos = new List<ChatTemplateDto>();
        foreach (var chatTemplate in chatTemplates)
        {
            chatTemplateDtos.Add(new ChatTemplateDto
            {
                TemplateName = chatTemplate.TemplateName,
                Sequence = chatTemplate.Sequence,
                Content = chatTemplate.Content
            });
        }

        var response = new GetChatTemplateResponse(chatTemplateDtos);
        return Result<GetChatTemplateResponse>.Ok(response);
    }
}
