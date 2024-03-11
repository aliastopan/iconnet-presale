namespace IConnet.Presale.Shared.Interfaces.Models.Identity;

public interface IRegistrationModel
{
   string Username { get; }
   string Password { get; }
   string EmploymentStatus { get; }
   string UserRole { get; }
   string JobTitle { get; }
   bool IsManagedByAdministrator { get; }
}
