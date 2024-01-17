using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Shared.Contracts.Identity.SetUserRole;

public record SetRoleRequest(Guid AuthorityAccountId, string AccessPassword, Guid SubjectAccountId, string Role)
    : IRoleUpdateModel;