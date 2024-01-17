using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Application.Identity.Commands.SetUserRole;

public class SetUserRoleCommand(Guid authorityAccountId, string accessPassword, Guid subjectAccountId, string role)
    : IRoleUpdateModel, IRequest<Result>
{
    [Required]
    public Guid AuthorityAccountId { get; } = authorityAccountId;

    [Required]
    public Guid SubjectAccountId { get; } = subjectAccountId;

    [Required]
    public string AccessPassword { get; } = accessPassword;

    [Required]
    [EnumDataType(typeof(Domain.Enums.UserRole))]
    public string Role { get; } = role;
}
