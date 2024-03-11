namespace IConnet.Presale.Shared.Contracts.Identity.Registration;

public record SignUpResponse(Guid UserAccountId,
    string Username,
    string EmploymentStatus,
    string UserRole,
    List<string> UserPrivileges,
    string JobTitle);