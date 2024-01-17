using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Shared.Contracts.Identity.Authentication;

public record RefreshAccessRequest(string AccessToken, string RefreshTokenStr) : IRefreshAccessModel;
