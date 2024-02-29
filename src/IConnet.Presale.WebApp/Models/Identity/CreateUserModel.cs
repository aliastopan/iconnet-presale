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
    [RegularExpression(RegexPattern.StrongPassword, ErrorMessage = "Password harus mengandung setidaknya satu digit, satu huruf kecil, satu huruf besar, dan setidaknya terdiri dari 8 karakter.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Konfirmasi password tidak boleh kosong.")]
    [Compare(nameof(Password), ErrorMessage = "Password tidak sesuai.")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string EmploymentStatus { get; set; }

    [Required]
    public string UserRole { get; set; }

    [Required(ErrorMessage = "Jabatan tidak boleh kosong.")]
    public string JobTitle { get; set; }

    [Required]
    public string JobShift { get; set; }

    [Required]
    public bool IsManagedByAdministrator { get; set; } = false;

    public void Reset()
    {
        Username = null;
        Password = null;
        ConfirmPassword = null;
        JobTitle = null;
    }
}
