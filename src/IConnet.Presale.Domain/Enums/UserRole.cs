namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum UserRole
{
    Guest = 0,
    Helpdesk = 1,
    PAC = 2, // Planning Asset Coverage
    Administrator = 64
}
