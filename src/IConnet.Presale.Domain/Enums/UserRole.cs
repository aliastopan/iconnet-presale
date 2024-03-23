namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum UserRole
{
    PTL = 0,
    // Sales,
    Helpdesk = 8,
    PAC = 16,
    // Management
    Administrator = 32,
    SuperUser = 64
}

// original value
// PTL = 0,
// Helpdesk = 1,
// PAC = 2,
// Administrator = 4,
// SuperUser = 64