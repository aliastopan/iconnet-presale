namespace IConnet.Presale.Application.DirectApprovals.Commands.AddDirectApproval;

public class AddDirectApprovalCommandHandler : IRequestHandler<AddDirectApprovalCommand, Result>
{
    private readonly IDirectApprovalHandler _directApprovalHandler;

    public AddDirectApprovalCommandHandler(IDirectApprovalHandler directApprovalHandler)
    {
        _directApprovalHandler = directApprovalHandler;
    }

    public async ValueTask<Result> Handle(AddDirectApprovalCommand request,
        CancellationToken cancellationToken)
    {
        await _directApprovalHandler.AddDirectApprovalAsync(request.Order, request.Description);

        return Result.Ok();
    }
}
