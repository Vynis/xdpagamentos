using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class TerminalMap : IEntityTypeConfiguration<Terminal>
    {
        public void Configure(EntityTypeBuilder<Terminal> builder)
        {
            builder.ToTable("tb_terminais");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("ter_id");
            builder.Property(c => c.NumTerminal).HasColumnName("ter_num_terminal");
            builder.Property(c => c.Status).HasColumnName("ter_status");
            builder.Property(c => c.EstId).HasColumnName("ter_est_id");

            builder.HasOne(c => c.Estabelecimento).WithMany(c => c.ListaTerminais).HasForeignKey(c => c.EstId);
        }
    }
}
