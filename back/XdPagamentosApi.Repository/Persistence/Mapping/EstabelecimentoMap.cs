using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class EstabelecimentoMap : IEntityTypeConfiguration<Estabelecimento>
    {
        public void Configure(EntityTypeBuilder<Estabelecimento> builder)
        {
            builder.ToTable("tb_estabelecimentos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("est_id");
        }
    }
}
