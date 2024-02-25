using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Contracts.Identity.Registration;
using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Application.Identity.Commands.Registration;

public class SignUpCommand(string username, string firstName, string lastName,
    DateOnly dateOfBirth, string emailAddress, string password,
    string employmentStatus, string userRole, string jobTitle, string jobShift)
    : IRegistrationModel, IRequest<Result<SignUpResponse>>
{
    [Required]
    [RegularExpression(RegexPattern.Username)]
    public string Username { get; } = username;

    [Required]
    [RegularExpression(RegexPattern.NameFormat)]
    [MaxLength(64)]
    public string FirstName { get; } = firstName;

    [MaxLength(64)]
    [RegularExpression(RegexPattern.NameFormat)]
    public string LastName { get; } = lastName;

    [Required]
    public DateOnly DateOfBirth { get; } = dateOfBirth;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; } = emailAddress;

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
}
