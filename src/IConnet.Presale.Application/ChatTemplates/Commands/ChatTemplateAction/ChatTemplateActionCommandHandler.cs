using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.ChatTemplates.Commands.ChatTemplateAction;

public class ChatTemplateActionCommandHandler : IRequestHandler<ChatTemplateActionCommand, Result>
{
    private readonly IChatTemplateHandler _chatTemplateHandler;

    public ChatTemplateActionCommandHandler(IChatTemplateHandler chatTemplateHandler)
    {
        _chatTemplateHandler = chatTemplateHandler;
    }

    public async ValueTask<Result> Handle(ChatTemplateActionCommand request,
        CancellationToken cancellationToken)
    {
        // data annotation validations
        var isInvalid = !request.TryValidate(out var errors);
        if (isInvalid)
        {
            return Result.Invalid(errors);
        }

        var tryChatTemplateAction = await _chatTemplateHandler.TryChatTemplateAction(request.ChatTemplateId,
            request.TemplateName,
            request.Sequence,
            request.Content,
            request.Action);

        if (tryChatTemplateAction.IsFailure())
        {
            return Result.Inherit(result: tryChatTemplateAction);
        }

        return Result.Ok();
    }
}
