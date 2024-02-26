using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.ChatTemplates.Queries;

public class GetChatTemplatesQueryHandler : IRequestHandler<GetChatTemplatesQuery, Result<GetChatTemplatesResponse>>
{
    private readonly IChatTemplateManager _chatTemplateManager;

    public GetChatTemplatesQueryHandler(IChatTemplateManager chatTemplateManager)
    {
        _chatTemplateManager = chatTemplateManager;
    }

    public async ValueTask<Result<GetChatTemplatesResponse>> Handle(GetChatTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        // data annotation validations
        var isInvalid = !request.TryValidate(out var errors);
        if (isInvalid)
        {
            return Result<GetChatTemplatesResponse>.Invalid(errors);
        }

        // chat template
        var tryGetChatTemplates = await _chatTemplateManager.TryGetChatTemplatesAsync(request.TemplateName);
        if (tryGetChatTemplates.IsFailure())
        {
            return Result<GetChatTemplatesResponse>.Inherit(result: tryGetChatTemplates);
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

        var response = new GetChatTemplatesResponse(chatTemplateDtos);
        return Result<GetChatTemplatesResponse>.Ok(response);
    }
}
