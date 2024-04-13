
namespace IConnet.Presale.Application.DirectApprovals.Commands.ToggleSoftDeletion;

public class ToggleDirectApprovalSoftDeletionCommandHandler : IRequestHandler<ToggleDirectApprovalSoftDeletionCommand, Result>
{
    private readonly IDirectApprovalHandler _directApprovalHandler;

    public ToggleDirectApprovalSoftDeletionCommandHandler(IDirectApprovalHandler directApprovalHandler)
    {
        _directApprovalHandler = directApprovalHandler;
    }

    public async ValueTask<Result> Handle(ToggleDirectApprovalSoftDeletionCommand request,
        CancellationToken cancellationToken)
    {
        await _directApprovalHandler.ToggleSoftDeletionAsync(request.DirectApprovalId, request.IsDeleted);

        return Result.Ok();
    }
}
