namespace IConnet.Presale.Application.RootCauses.Commands.ToggleSoftDeletion;

public class ToggleRootCauseSoftDeletionCommandHandler : IRequestHandler<ToggleRootCauseSoftDeletionCommand, Result>
{
    private readonly IRootCauseHandler _rootCauseHandler;

    public ToggleRootCauseSoftDeletionCommandHandler(IRootCauseHandler rootCauseHandler)
    {
        _rootCauseHandler = rootCauseHandler;
    }

    public async ValueTask<Result> Handle(ToggleRootCauseSoftDeletionCommand request,
        CancellationToken cancellationToken)
    {
        await _rootCauseHandler.ToggleSoftDeletionAsync(request.RootCauseId, request.IsDeleted);

        return Result.Ok();
    }
}
