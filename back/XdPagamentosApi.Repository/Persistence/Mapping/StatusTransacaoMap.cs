using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class StatusTransacaoMap : IEntityTypeConfiguration<StatusTransacao>
    {
        public void Configure(EntityTypeBuilder<StatusTransacao> builder)
        {
            builder.ToTable("tb_status_transacao");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("stt_id");
            builder.Property(c => c.Codigo).HasColumnName("stt_codigo");
            builder.Property(c => c.DescStatus).HasColumnName("stt_desc_status");
            builder.Property(c => c.Tipo).HasColumnName("stt_tipo");

        }
    }
}
