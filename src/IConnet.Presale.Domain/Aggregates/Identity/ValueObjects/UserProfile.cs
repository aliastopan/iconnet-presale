#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Identity;

public class UserProfile
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateOnly DateOfBirth { get ; init; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
