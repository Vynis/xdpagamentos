using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class RelContaEstabelecimentoMap : IEntityTypeConfiguration<RelContaEstabelecimento>
    {
        public void Configure(EntityTypeBuilder<RelContaEstabelecimento> builder)
        {
            builder.ToTable("tb_rel_conta_estabelecimento");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("rce_id");
            builder.Property(c => c.CocId).HasColumnName("rce_coc_id");
            builder.Property(c => c.EstId).HasColumnName("rce_est_id");

            builder.HasOne(c => c.ContaCaixa).WithMany(c => c.ListaRelContaEstabelecimento).HasForeignKey(c => c.CocId);
            builder.HasOne(c => c.Estabelecimento).WithMany(c => c.ListaRelContaEstabelecimento).HasForeignKey(c => c.EstId);
        }
    }
}
