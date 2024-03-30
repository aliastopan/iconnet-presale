using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Persistence.Configurations.Common;

internal sealed class DirectApprovalConfiguration : IEntityTypeConfiguration<DirectApproval>
{
    public void Configure(EntityTypeBuilder<DirectApproval> builder)
    {
        builder.ToTable("dbo.direct_approval");

        // primary key
        builder.HasKey(da => da.DirectApprovalId);

        // configure properties
        builder.Property(da => da.DirectApprovalId)
                .HasColumnName("id")
                .IsRequired();

        builder.Property(da => da.Order)
                .HasColumnName("order");

        builder.Property(da => da.Description)
                .HasColumnName("description")
                .IsRequired();

        builder.Property(da => da.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);
    }
}
