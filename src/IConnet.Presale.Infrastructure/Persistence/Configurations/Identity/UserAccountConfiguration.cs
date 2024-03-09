using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Aggregates.Identity.ValueObjects;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.Infrastructure.Persistence.Configurations.Identity;

internal sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("dbo.user_account");

        // primary key
        builder.HasKey(u => u.UserAccountId);

        // configure properties
        builder.Property(u => u.UserAccountId)
            .HasColumnName("user_account_id")
            .HasMaxLength(36)
            .IsRequired();

        builder.ComplexProperty(u => u.User,
            user =>
            {
                user.IsRequired();

                user.Property(u => u.Username)
                    .HasColumnName("username")
                    .HasMaxLength(32)
                    .IsRequired();

                user.Property(u => u.EmploymentStatus)
                    .HasColumnName("employment_status")
                    .IsRequired();

                user.Property(u => u.UserRole)
                    .HasColumnName("user_role")
                    .IsRequired();

                user.Property(u => u.UserPrivileges)
                    .HasColumnName("user_privileges")
                    .HasConversion(
                        x => string.Join(',', x.Select(privilege => privilege.ToString())),
                        x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(privilegeStr => Enum.Parse<UserPrivilege>(privilegeStr))
                                .ToList(),
                        new ValueComparer<IReadOnlyCollection<UserPrivilege>>(
                            (c1, c2) => c1!.SequenceEqual(c2!),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToList()))
                    .IsRequired();

                user.Property(u => u.JobTitle)
                    .HasColumnName("job_title")
                    .IsRequired();

                user.Property(u => u.JobShift)
                    .HasColumnName("job_shift")
                    .HasMaxLength(32)
                    .IsRequired();
            });

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(96) // SHA384 (48-byte)
            .IsRequired();

        builder.Property(u => u.PasswordSalt)
            .HasColumnName("password_salt")
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(u => u.CreationDate)
            .HasColumnName("creation_date")
            .IsRequired();

        builder.Property(u => u.LastSignedIn)
            .HasColumnName("last_signed_in")
            .IsRequired();

        // configure relationships
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.UserAccount)
            .HasForeignKey(rt => rt.FkUserAccountId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
