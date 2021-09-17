using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class OperadoraMap : IEntityTypeConfiguration<Operadora>
    {
        public void Configure(EntityTypeBuilder<Operadora> builder)
        {
            builder.ToTable("tb_operadoras");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("ope_id");
            builder.Property(c => c.Nome).HasColumnName("ope_nome");
            builder.Property(c => c.Status).HasColumnName("ope_status");
        }
    }
}
