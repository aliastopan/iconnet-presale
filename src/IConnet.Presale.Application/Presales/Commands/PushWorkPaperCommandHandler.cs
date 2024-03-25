
namespace IConnet.Presale.Application.Presales.Commands;

public class PushWorkPaperCommandHandler : IRequestHandler<PushWorkPaperCommand, Result>
{
    private readonly IWorkPaperAggregateHandler _workPaperAggregateHandler;

    public PushWorkPaperCommandHandler(IWorkPaperAggregateHandler workPaperAggregateHandler)
    {
        _workPaperAggregateHandler = workPaperAggregateHandler;
    }

    public async ValueTask<Result> Handle(PushWorkPaperCommand request,
        CancellationToken cancellationToken)
    {
        bool isValid = request.TryValidate(out var errors);

        if (!isValid)
        {
            return Result.Invalid(errors);
        }

        Result tryInsertOrUpdateWorkPaper = await _workPaperAggregateHandler.TryInsertOrUpdateWorkPaperAsync(request);

        if (tryInsertOrUpdateWorkPaper.IsFailure())
        {
            return Result.Inherit(result: tryInsertOrUpdateWorkPaper);
        }

        return Result.Ok();
    }
}
