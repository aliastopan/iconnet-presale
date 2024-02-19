using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Persistence.Configurations.Common;

internal sealed class ChatTemplateConfiguration
{
    public void Configure(EntityTypeBuilder<ChatTemplate> builder)
    {
        builder.ToTable("dbo.chat_template");

        // primary key
        builder.HasKey(ct => ct.ChatTemplateId);

        // configure properties
        builder.Property(ct => ct.ChatTemplateId)
               .HasColumnName("chat_template_id")
               .IsRequired();

        builder.Property(ct => ct.TemplateName)
               .HasColumnName("template_name")
               .IsRequired()
               .HasMaxLength(32);

        builder.Property(ct => ct.Sequence)
               .HasColumnName("sequence")
               .IsRequired();

        builder.Property(ct => ct.Content)
               .HasColumnName("content")
               .IsRequired();
    }
}
