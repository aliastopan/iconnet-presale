using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.ChatTemplates.Queries;

public class GetChatTemplatesQueryHandler : IRequestHandler<GetChatTemplatesQuery, Result<GetChatTemplatesQueryResponse>>
{
    private readonly IChatTemplateHandler _chatTemplateHandler;

    public GetChatTemplatesQueryHandler(IChatTemplateHandler chatTemplateHandler)
    {
        _chatTemplateHandler = chatTemplateHandler;
    }

    public ValueTask<Result<GetChatTemplatesQueryResponse>> Handle(GetChatTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        Result<GetChatTemplatesQueryResponse> result;

        // data annotation validations
        var isInvalid = !request.TryValidate(out var errors);
        if (isInvalid)
        {
            result = Result<GetChatTemplatesQueryResponse>.Invalid(errors);
            return ValueTask.FromResult(result);
        }

        // chat template
        var tryGetChatTemplates = _chatTemplateHandler.TryGetChatTemplates(request.TemplateName);
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
