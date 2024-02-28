namespace IConnet.Presale.Shared.Contracts.Identity.Registration;

public record SignUpResponse(Guid UserAccountId,
    string Username,
    string EmailAddress,
    string EmploymentStatus,
    string UserRole,
    List<string> UserPrivileges,
    string JobTitle,
    string JobShift);