namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum UserRole
{
    PTL = 0,
    Sales = 1,
    Helpdesk = 8,
    PAC = 16,
    Management = 30,
    Administrator = 32,
    SuperUser = 64
}