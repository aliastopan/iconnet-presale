namespace IConnet.Presale.Shared.Interfaces.Models.Identity;

public interface IRegistrationModel
{
   string Username { get; }
   string FirstName { get; }
   string LastName { get; }
   DateOnly DateOfBirth { get; }
   string EmailAddress { get; }
   string Password { get; }
   string EmploymentStatus { get; }
   string UserRole { get; }
   string JobTitle { get; }
   string JobShift { get; }
   bool IsManagedByAdministrator { get; }
}
