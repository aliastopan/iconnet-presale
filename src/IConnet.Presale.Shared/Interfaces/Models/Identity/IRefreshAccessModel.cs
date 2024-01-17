namespace IConnet.Presale.Shared.Interfaces.Models.Identity;

public interface IRefreshAccessModel
{
    string AccessToken { get; }
    string RefreshTokenStr { get; }
}
