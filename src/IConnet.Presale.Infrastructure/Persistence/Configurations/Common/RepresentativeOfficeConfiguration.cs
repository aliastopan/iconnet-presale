using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Persistence.Configurations.Common;

internal sealed class RepresentativeOfficeConfiguration : IEntityTypeConfiguration<RepresentativeOffice>
{
    public void Configure(EntityTypeBuilder<RepresentativeOffice> builder)
    {
        builder.ToTable("dbo.kantor_perwakilan");

        // primary key
        builder.HasKey(ro => ro.KantorPerwakilanId);

        // configure properties
        builder.Property(ro => ro.KantorPerwakilanId)
                .HasColumnName("id")
                .IsRequired();

        builder.Property(ro => ro.Order)
                .HasColumnName("order");

        builder.Property(ro => ro.Perwakilan)
                .HasColumnName("perwakilan")
                .IsRequired();

        builder.Property(ro => ro.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);
    }
}
