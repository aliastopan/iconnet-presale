namespace IConnet.Presale.Application.Identity.Commands.EditUserAccount;

public class EditUserAccountCommandHandler : IRequestHandler<EditUserAccountCommand, Result>
{
    private readonly IIdentityManager _identityManager;

    public EditUserAccountCommandHandler(IIdentityManager identityManager)
    {
        _identityManager = identityManager;
    }

    public async ValueTask<Result> Handle(EditUserAccountCommand request,
        CancellationToken cancellationToken)
    {
        // data annotation validations
        var isInvalid = !request.TryValidate(out var errors);
        if (isInvalid)
        {
            return Result.Invalid(errors);
        }

        // edit user account
        var tryEditUserAccount = await _identityManager.TryEditUserAccount(request.UserAccountId,
            request.NewUsername, request.NewPassword, request.IsChangeUsername, request.IsChangePassword);

        if (tryEditUserAccount.IsFailure())
        {
            return Result.Inherit(result: tryEditUserAccount);
        }

        return Result.Ok();
    }
}
