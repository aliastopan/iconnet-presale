using System.Security.Cryptography;
using System.Text;
using SecretSauce = CCred.Sauce;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class PasswordProvider : IPasswordService
{
    public string HashPassword(string password, out string passwordSalt)
    {
        passwordSalt = SecretSauce.GenerateSalt(length: 16);
        return SecretSauce.GetHash<SHA384>(password, passwordSalt, Encoding.UTF8);
    }

    public bool VerifyPassword(string password, string passwordSalt, string passwordHash)
    {
        return SecretSauce.Verify<SHA384>(password, passwordSalt, passwordHash, Encoding.UTF8);
    }
}
