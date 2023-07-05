using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class PlanoContaMap : IEntityTypeConfiguration<PlanoConta>
    {
        public void Configure(EntityTypeBuilder<PlanoConta> builder)
        {
            builder.ToTable("tb_plano_conta");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("pco_id");
            builder.Property(c => c.Tipo).HasColumnName("pco_tipo");
            builder.Property(c => c.Obs).HasColumnName("pco_obs");
            builder.Property(c => c.Descricao).HasColumnName("pco_descricao");
            builder.Property(c => c.Referencia).HasColumnName("pco_referencia");
            builder.Property(c => c.Status).HasColumnName("pco_status");
            builder.Property(c => c.Status).HasColumnName("pco_status");
        }
    }
}
