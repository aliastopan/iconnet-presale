using System.Security.Claims;
using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Services;

public sealed class SessionService
{
    public UserModel? UserModel { get; private set; }
    public bool HasUser => UserModel is not null;

    public void SetSession(ClaimsPrincipal principal)
    {
        UserModel = new UserModel(principal);
    }

    public void UnsetSession()
    {
        UserModel = null;
    }
}
