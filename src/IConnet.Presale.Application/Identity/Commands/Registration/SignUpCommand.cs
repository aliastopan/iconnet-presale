using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Contracts.Identity.Registration;
using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Application.Identity.Commands.Registration;

public class SignUpCommand(string username, string password,
    string employmentStatus, string userRole, string jobTitle, string jobShift,
    bool isManagedByAdministrator)
    : IRegistrationModel, IRequest<Result<SignUpResponse>>
{
    [Required]
    [RegularExpression(RegexPattern.Username)]
    public string Username { get; } = username;

    [Required]
    [RegularExpression(RegexPattern.StrongPassword)]
    public string Password { get; } = password;

    [Required]
    public string UserRole { get; } = userRole;

    [Required]
    public string EmploymentStatus { get; } = employmentStatus;

    [Required]
    public string JobTitle { get; } = jobTitle;

    [Required]
    public string JobShift { get; } = jobShift;

    [Required]
    public bool IsManagedByAdministrator { get; } = isManagedByAdministrator;
}
