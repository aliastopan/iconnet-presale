using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Application.Identity.Commands.GrantUserPrivilege;

public class GrantPrivilegeCommand(Guid authorityAccountId, string accessPassword, Guid subjectAccountId, string privilege)
    : IPrivilegeUpdateModel, IRequest<Result>
{
    [Required]
    public Guid AuthorityAccountId { get; } = authorityAccountId;

    [Required]
    public Guid SubjectAccountId { get; } = subjectAccountId;

    [Required]
    public string AccessPassword { get; } = accessPassword;

    [Required]
    [EnumDataType(typeof(Domain.Enums.UserPrivilege))]
    public string Privilege { get; } = privilege;
}
