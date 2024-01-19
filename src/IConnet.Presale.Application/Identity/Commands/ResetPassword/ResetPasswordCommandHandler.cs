using IConnet.Presale.Application.Common.Interfaces.Managers;

namespace IConnet.Presale.Application.Identity.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IIdentityManager _identityManager;

    public ResetPasswordCommandHandler(IIdentityManager identityManager)
    {
        _identityManager = identityManager;
    }

    public async ValueTask<Result> Handle(ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        // data annotation validations
        var isInvalid = !request.TryValidate(out var errors);
        if (isInvalid)
        {
            var invalid = Result.Invalid(errors);
            return await ValueTask.FromResult(invalid);
        }

        // reset password
        var tryResetPassword = await _identityManager.TryResetPasswordAsync(request.UserAccountId,
            request.OldPassword,
            request.NewPassword);

        if (tryResetPassword.IsFailure())
        {
            var failure = Result.Inherit(result: tryResetPassword);
            return await ValueTask.FromResult(failure);
        }

        var ok = Result.Ok();
        return await ValueTask.FromResult(ok);
    }
}
