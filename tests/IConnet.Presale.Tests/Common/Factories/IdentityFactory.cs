using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Aggregates.Identity.ValueObjects;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.Tests.Common.Factories;

public static class IdentityFactory
{
    public static UserAccount GetTestUserAccount()
    {
        return new UserAccount
        {
            UserAccountId = Guid.Parse("5d771905-c325-4f9a-adb8-954e0ae21860"),
            User = new User
            {
                Username = "tester",
                UserRole = UserRole.PTL,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer
                },
                JobTitle = "Tester"
            },
            PasswordHash = "15ebbed109775ec3bf1a1be98871dfbcb534f593bda7be6269db573efa4822065772cc6de99313b194d4321954372bf0",
            PasswordSalt = "14O2U0D902x96xZR",
            CreationDate = DateTimeOffset.Now,
            LastSignedIn = DateTimeOffset.Now
        };
    }
}
