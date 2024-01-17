#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Identity;

public class RefreshToken
{
    public Guid RefreshTokenId { get; set; }
    public string Token { get; set; }
    public string Jti { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
    public bool IsUsed { get; set; }
    public bool IsInvalidated { get; set; }
    public bool IsDeleted { get; set; }

    public Guid FkUserAccountId { get; set; }

    public virtual UserAccount UserAccount { get; set;}
}
