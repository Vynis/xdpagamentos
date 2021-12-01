using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class TipoOperacaoMap : IEntityTypeConfiguration<TipoOperacao>
    {
        public void Configure(EntityTypeBuilder<TipoOperacao> builder)
        {
            builder.ToTable("tb_tipo_operacao");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("tio_id");
            builder.Property(c => c.OpeId).HasColumnName("tio_ope_id");
            builder.Property(c => c.Codigo).HasColumnName("tio_codigo");
            builder.Property(c => c.Descricao).HasColumnName("tio_descricao");
            builder.Property(c => c.Ref).HasColumnName("tio_ref");

            builder.HasOne(c => c.Operadora).WithMany(c => c.ListaTipoOperacao).HasForeignKey(c => c.OpeId);

        }
    }
}
