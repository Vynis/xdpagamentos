using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class TipoTransacaoMap : IEntityTypeConfiguration<TipoTransacao>
    {
        public void Configure(EntityTypeBuilder<TipoTransacao> builder)
        {
            builder.ToTable("tb_tipo_transacao");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("tit_id");
            builder.Property(c => c.QtdParcelas).HasColumnName("tit_qtd_parcelas");
            builder.Property(c => c.PercDesconto).HasColumnName("tit_perc_desconto");
            builder.Property(c => c.Status).HasColumnName("tit_status");
            builder.Property(c => c.CliId).HasColumnName("tit_cli_id");

            builder.HasOne(c => c.Cliente).WithMany(c => c.ListaTipoTransacao).HasForeignKey(c => c.CliId);
        }
    }
}
