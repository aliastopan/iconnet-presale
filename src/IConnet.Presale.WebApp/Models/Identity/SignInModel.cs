#nullable disable
using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.WebApp.Models.Identity;

public class SignInModel : IAuthenticationModel
{
    [Required(ErrorMessage = "Username tidak boleh kosong.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password tidak boleh kosong.")]
    public string Password { get; set; }
}
