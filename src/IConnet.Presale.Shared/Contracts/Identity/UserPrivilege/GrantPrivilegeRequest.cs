using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Shared.Contracts.Identity.UserPrivilege;

public record GrantPrivilegeRequest(Guid AuthorityAccountId, string AccessPassword, Guid SubjectAccountId, string Privilege)
    : IPrivilegeUpdateModel;