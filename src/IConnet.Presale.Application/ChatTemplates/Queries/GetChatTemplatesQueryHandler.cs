using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.ChatTemplates.Queries;

public class GetChatTemplatesQueryHandler : IRequestHandler<GetChatTemplatesQuery, Result<GetChatTemplatesResponse>>
{
    private readonly IChatTemplateManager _chatTemplateManager;

    public GetChatTemplatesQueryHandler(IChatTemplateManager chatTemplateManager)
    {
        _chatTemplateManager = chatTemplateManager;
    }

    public ValueTask<Result<GetChatTemplatesResponse>> Handle(GetChatTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        Result<GetChatTemplatesResponse> result;

        // data annotation validations
        var isInvalid = !request.TryValidate(out var errors);
        if (isInvalid)
        {
            result = Result<GetChatTemplatesResponse>.Invalid(errors);
            return ValueTask.FromResult(result);
        }

        // chat template
        var tryGetChatTemplates = _chatTemplateManager.TryGetChatTemplates(request.TemplateName);
        if (tryGetChatTemplates.IsFailure())
        {
            result = Result<GetChatTemplatesResponse>.Inherit(result: tryGetChatTemplates);
            return ValueTask.FromResult(result);
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
        result = Result<GetChatTemplatesResponse>.Ok(response);

        return ValueTask.FromResult(result);
    }
}
