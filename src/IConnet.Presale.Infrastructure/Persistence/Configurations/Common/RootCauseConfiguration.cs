using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Persistence.Configurations.Common;

internal sealed class RootCauseConfiguration : IEntityTypeConfiguration<RootCause>
{
    public void Configure(EntityTypeBuilder<RootCause> builder)
    {
        builder.ToTable("dbo.root_cause");

        // primary key
        builder.HasKey(rc => rc.RootCauseId);

        // configure properties
        builder.Property(rc => rc.RootCauseId)
                .HasColumnName("id")
                .IsRequired();

        builder.Property(rc => rc.Order)
                .HasColumnName("order");

        builder.Property(rc => rc.Cause)
                .HasColumnName("root_cause")
                .IsRequired();

        builder.Property(rc => rc.Classification)
                .HasColumnName("classification");

        builder.Property(rc => rc.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

        builder.Property(rc => rc.IsOnVerification)
                .HasColumnName("is_on_verification")
                .HasDefaultValue(false);
    }
}
