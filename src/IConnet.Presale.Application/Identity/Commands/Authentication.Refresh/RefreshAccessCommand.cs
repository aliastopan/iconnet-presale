using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Contracts.Identity.Authentication;
using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Application.Identity.Commands.Authentication;

public class RefreshAccessCommand(string accessToken, string refreshTokenStr)
    : IRefreshAccessModel, IRequest<Result<RefreshAccessResponse>>
{
    [Required]
    public string AccessToken { get; set; } = accessToken;

    [Required]
    public string RefreshTokenStr { get; set; } = refreshTokenStr;
}
