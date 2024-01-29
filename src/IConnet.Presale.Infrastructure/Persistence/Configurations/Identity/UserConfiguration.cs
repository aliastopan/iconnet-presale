using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.Infrastructure.Persistence.Configurations.Identity;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("dbo.user");

        // primary key
        builder.HasKey(u => u.UserId);

        // configure properties
        builder.Property(u => u.UserId)
            .HasColumnName("user_id")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(u => u.EmailAddress)
            .HasColumnName("email_address")
            .HasMaxLength(255);

        builder.Property(u => u.UserRole)
            .HasColumnName("user_role")
            .IsRequired();

        builder.Property(u => u.UserPrivileges)
            .HasColumnName("user_privileges")
            .HasConversion(
                x => string.Join(',', x.Select(privilege => privilege.ToString())),
                x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(privilegeStr => Enum.Parse<UserPrivilege>(privilegeStr))
                        .ToList(),
                new ValueComparer<ICollection<UserPrivilege>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()))
            .IsRequired();

        builder.Property(u => u.JobTitle)
            .HasColumnName("job_title")
            .HasMaxLength(64);

        builder.Property(u => u.JobShift)
            .HasColumnName("job_shift")
            .IsRequired();
    }
}
