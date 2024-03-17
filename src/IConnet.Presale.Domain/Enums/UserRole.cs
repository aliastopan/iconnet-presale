namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum UserRole
{
    PTL = 0,
    Helpdesk = 1,
    PAC = 2,
    Administrator = 4,
    SuperUser = 64
}
