namespace IConnet.Presale.Application.RootCauses.Commands.AddRootCause;

public class AddRootCauseCommandHandler : IRequestHandler<AddRootCauseCommand, Result>
{
    private readonly IRootCauseHandler _rootCauseHandler;

    public AddRootCauseCommandHandler(IRootCauseHandler rootCauseHandler)
    {
        _rootCauseHandler = rootCauseHandler;
    }

    public async ValueTask<Result> Handle(AddRootCauseCommand request,
        CancellationToken cancellationToken)
    {
        await _rootCauseHandler.AddRootCauseAsync(request.Order, request.Cause, request.Classification);

        return Result.Ok();
    }
}
