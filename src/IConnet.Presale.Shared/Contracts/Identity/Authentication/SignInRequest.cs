using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Shared.Contracts.Identity.Authentication;

public record SignInRequest(string Username, string Password) : IAuthenticationModel;
