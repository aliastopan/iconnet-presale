
namespace IConnet.Presale.Application.RootCauses.Commands.EditRootCause;

public class EditRootCauseCommandHandler : IRequestHandler<EditRootCauseCommand, Result>
{
    private readonly IRootCauseHandler _rootCauseHandler;

    public EditRootCauseCommandHandler(IRootCauseHandler rootCauseHandler)
    {
        _rootCauseHandler = rootCauseHandler;
    }

    public async ValueTask<Result> Handle(EditRootCauseCommand request,
        CancellationToken cancellationToken)
    {
        await _rootCauseHandler.UpdateRootCauseAsync(request.RootCauseId, request.RootCause, request.Classification);

        return Result.Ok();
    }
}
