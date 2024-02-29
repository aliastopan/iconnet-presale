#nullable disable
using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Interfaces.Models.Identity;
using IConnet.Presale.Shared.Validations;

namespace IConnet.Presale.WebApp.Models.Identity;

public class CreateUserModel : IRegistrationModel
{
    [Required(ErrorMessage = "Username tidak boleh kosong.")]
    [RegularExpression(RegexPattern.Username)]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password tidak boleh kosong.")]
    [RegularExpression(RegexPattern.StrongPassword)]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Password tidak sesuai.")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string EmploymentStatus { get; set; }

    [Required]
    public string UserRole { get; set; }

    [Required]
    public string JobTitle { get; set; }

    [Required]
    public string JobShift { get; set; }

    [Required]
    public bool IsManagedByAdministrator { get; set; } = false;
}
