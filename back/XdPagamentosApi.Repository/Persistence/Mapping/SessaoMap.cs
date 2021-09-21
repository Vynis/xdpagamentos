using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class SessaoMap : IEntityTypeConfiguration<Sessao>
    {
        public void Configure(EntityTypeBuilder<Sessao> builder)
        {
            builder.ToTable("tb_sessoes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("ses_id");
            builder.Property(c => c.Descricao).HasColumnName("ses_descricao");
            builder.Property(c => c.Referencia).HasColumnName("ses_ref");
        }
    }
}
