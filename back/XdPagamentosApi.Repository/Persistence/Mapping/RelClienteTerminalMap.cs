using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class RelClienteTerminalMap : IEntityTypeConfiguration<RelClienteTerminal>
    {
        public void Configure(EntityTypeBuilder<RelClienteTerminal> builder)
        {
            builder.ToTable("tb_rel_clientes_terminais");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("rct_id");
            builder.Property(c => c.CliId).HasColumnName("rct_cli_id");
            builder.Property(c => c.TerId).HasColumnName("rct_ter_id");

            builder.HasOne(c => c.Cliente).WithMany(c => c.ListaRelClienteTerminal).HasForeignKey(c => c.CliId);
            builder.HasOne(c => c.Terminal).WithMany(c => c.ListaRelClienteTerminal).HasForeignKey(c => c.TerId);
        }
    }
}
