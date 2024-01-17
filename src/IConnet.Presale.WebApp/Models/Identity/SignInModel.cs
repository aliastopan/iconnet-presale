#nullable disable
using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.WebApp.Models.Identity;

public class SignInModel : IAuthenticationModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
