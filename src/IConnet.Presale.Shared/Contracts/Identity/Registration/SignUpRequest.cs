using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Shared.Contracts.Identity.Registration;

public record SignUpRequest(string Username, string FirstName, string LastName,
    DateOnly DateOfBirth, string EmailAddress, string Password,
    string EmploymentStatus, string UserRole, string JobTitle, string JobShift)
    : IRegistrationModel;