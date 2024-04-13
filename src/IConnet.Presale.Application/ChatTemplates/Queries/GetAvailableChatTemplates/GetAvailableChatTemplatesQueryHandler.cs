using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.ChatTemplates.Queries.GetAvailableChatTemplates;

public class GetAvailableChatTemplatesQueryHandler : IRequestHandler<GetAvailableChatTemplatesQuery, Result<GetChatTemplatesQueryResponse>>
{
    private readonly IChatTemplateHandler _chatTemplateHandler;

    public GetAvailableChatTemplatesQueryHandler(IChatTemplateHandler chatTemplateHandler)
    {
        _chatTemplateHandler = chatTemplateHandler;
    }

    public ValueTask<Result<GetChatTemplatesQueryResponse>> Handle(GetAvailableChatTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        Result<GetChatTemplatesQueryResponse> result;

        // chat template
        var tryGetChatTemplates = _chatTemplateHandler.TryGetAvailableChatTemplates();
        if (tryGetChatTemplates.IsFailure())
        {
            result = Result<GetChatTemplatesQueryResponse>.Inherit(result: tryGetChatTemplates);
            return ValueTask.FromResult(result);
        }

        var chatTemplates = tryGetChatTemplates.Value;

        var chatTemplateDtos = new List<ChatTemplateDto>();
        foreach (var chatTemplate in chatTemplates)
        {
            chatTemplateDtos.Add(new ChatTemplateDto
            {
                ChatTemplateId = chatTemplate.ChatTemplateId,
                TemplateName = chatTemplate.TemplateName,
                Sequence = chatTemplate.Sequence,
                Content = chatTemplate.Content
            });
        }

        var response = new GetChatTemplatesQueryResponse(chatTemplateDtos);
        result = Result<GetChatTemplatesQueryResponse>.Ok(response);

        return ValueTask.FromResult(result);
    }
}
