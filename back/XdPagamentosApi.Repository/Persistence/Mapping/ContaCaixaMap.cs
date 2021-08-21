using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class ContaCaixaMap : IEntityTypeConfiguration<ContaCaixa>
    {
        public void Configure(EntityTypeBuilder<ContaCaixa> builder)
        {
            builder.ToTable("tb_conta_caixa");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("coc_id");
            builder.Property(c => c.Descricao).HasColumnName("coc_descricao");
            builder.Property(c => c.Status).HasColumnName("coc_status");
        }
    }
}
