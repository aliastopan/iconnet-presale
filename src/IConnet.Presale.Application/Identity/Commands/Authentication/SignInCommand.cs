using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Contracts.Identity.Authentication;
using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Application.Identity.Commands.Authentication;

public class SignInCommand(string username, string password)
    : IAuthenticationModel, IRequest<Result<SignInResponse>>
{
    [Required]
    public string Username { get; } = username;

    [Required]
    public string Password { get; } = password;
}
