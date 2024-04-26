namespace IConnet.Presale.Application.RootCauses.Commands.ToggleOptions;

public class ToggleRootCauseOptionsCommandHandler : IRequestHandler<ToggleRootCauseOptionsCommand, Result>
{
    private readonly IRootCauseHandler _rootCauseHandler;

    public ToggleRootCauseOptionsCommandHandler(IRootCauseHandler rootCauseHandler)
    {
        _rootCauseHandler = rootCauseHandler;
    }

    public async ValueTask<Result> Handle(ToggleRootCauseOptionsCommand request,
        CancellationToken cancellationToken)
    {
        await _rootCauseHandler.ToggleOptionsAsync(request.RootCauseId, request.IsDeleted, request.IsOnVerification);

        return Result.Ok();
    }
}
