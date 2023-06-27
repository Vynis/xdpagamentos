using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class ContaReceberMap : IEntityTypeConfiguration<ContaReceber>
    {
        public void Configure(EntityTypeBuilder<ContaReceber> builder)
        {
            builder.ToTable("tb_contas_receber");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("cor_id");
            builder.Property(c => c.Descricao).HasColumnName("cor_descricao");
            builder.Property(c => c.Valor).HasColumnName("cor_valor");
            builder.Property(c => c.Data).HasColumnName("cor_data");
            builder.Property(c => c.Status).HasColumnName("cor_status");
            builder.Property(c => c.Obs).HasColumnName("cor_observacoes");
            builder.Property(c => c.DataCadastro).HasColumnName("cor_dt_cadastro");
            builder.Property(c => c.CecId).HasColumnName("cor_cec_id");

            builder.HasOne(c => c.CentroCusto).WithMany(c => c.ListContaReceber).HasForeignKey(c => c.CecId);
        }
    }
}
