using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class ContaPagarMap : IEntityTypeConfiguration<ContaPagar>
    {
        public void Configure(EntityTypeBuilder<ContaPagar> builder)
        {
            builder.ToTable("tb_contas_pagar");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("cpa_id");
            builder.Property(c => c.Descricao).HasColumnName("cpa_descricao");
            builder.Property(c => c.Valor).HasColumnName("cpa_valor");
            builder.Property(c => c.Data).HasColumnName("cpa_data");
            builder.Property(c => c.Status).HasColumnName("cpa_status");
            builder.Property(c => c.Obs).HasColumnName("cpa_observacoes");
            builder.Property(c => c.DataCadastro).HasColumnName("cpa_dt_cadastro");
            builder.Property(c => c.CecId).HasColumnName("cpa_cec_id");

            builder.HasOne(c => c.CentroCusto).WithMany(c => c.ListContaPagar).HasForeignKey(c => c.CecId);
        }
    }
}
