using IConnet.Presale.Shared.Contracts.Identity.Registration;

namespace IConnet.Presale.Application.Identity.Commands.Registration;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Result<SignUpResponse>>
{
    private readonly IIdentityManager _identityManager;

    public SignUpCommandHandler(IIdentityManager identityManager)
    {
        _identityManager = identityManager;
    }

    public async ValueTask<Result<SignUpResponse>> Handle(SignUpCommand request,
        CancellationToken cancellationToken)
    {
        // data annotation validations
        var isInvalid = !request.TryValidate(out var errors);
        if (isInvalid)
        {
            return Result<SignUpResponse>.Invalid(errors);
        }

        // registration
        var trySignUp = await _identityManager.TrySignUpAsync(request.Username,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.EmailAddress,
            request.Password,
            request.EmploymentStatus,
            request.UserRole,
            request.JobTitle,
            request.JobShift);

        if (trySignUp.IsFailure())
        {
            return Result<SignUpResponse>.Inherit(result: trySignUp);
        }

        var userAccount = trySignUp.Value;
        var response = new SignUpResponse(userAccount.UserAccountId,
            userAccount.User.Username,
            userAccount.UserProfile.FullName,
            userAccount.UserProfile.DateOfBirth,
            userAccount.User.EmailAddress,
            userAccount.User.EmploymentStatus.ToString(),
            userAccount.User.UserRole.ToString(),
            userAccount.User.UserPrivileges.Select(privilege => privilege.ToString()).ToList(),
            userAccount.User.JobTitle,
            userAccount.User.JobShift.ToString());

        return Result<SignUpResponse>.Ok(response);
    }
}
