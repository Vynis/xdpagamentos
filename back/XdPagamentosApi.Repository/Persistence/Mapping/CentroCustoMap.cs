using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class CentroCustoMap : IEntityTypeConfiguration<CentroCusto>
    {
        public void Configure(EntityTypeBuilder<CentroCusto> builder)
        {
            builder.ToTable("tb_centro_custo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("cec_id");
            builder.Property(c => c.Descricao).HasColumnName("cec_descricao");
            builder.Property(c => c.Status).HasColumnName("cec_status");
        }
    }
}
