using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class BancoMap : IEntityTypeConfiguration<Banco>
    {
        public void Configure(EntityTypeBuilder<Banco> builder)
        {
            builder.ToTable("tb_bancos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("ban_id");
            builder.Property(c => c.Numero).HasColumnName("ban_numero");
            builder.Property(c => c.Nome).HasColumnName("ban_nome");
            builder.Property(c => c.Status).HasColumnName("ban_status");
        }
    }
}
