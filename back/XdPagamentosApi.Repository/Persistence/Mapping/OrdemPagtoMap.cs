using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class OrdemPagtoMap : IEntityTypeConfiguration<OrdemPagto>
    {
        public void Configure(EntityTypeBuilder<OrdemPagto> builder)
        {
            builder.ToTable("tb_ordem_pagto");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("orp_id");
            builder.Property(c => c.FopId).HasColumnName("orp_fop_id");
            builder.Property(c => c.Valor).HasColumnName("orp_valor");
            builder.Property(c => c.NumIdentifcaocao).HasColumnName("orp_num_identificacao");
            builder.Property(c => c.Status).HasColumnName("orp_status");
            builder.Property(c => c.Chave).HasColumnName("orp_chave");
            builder.Property(c => c.DtEmissao).HasColumnName("orp_dt_emissao");
            builder.Property(c => c.DtBaixa).HasColumnName("orp_dt_baixa");
            builder.Property(c => c.EstId).HasColumnName("orp_est_id");

            builder.HasOne(c => c.Estabelecimento).WithMany(c => c.ListaOrdemPagos).HasForeignKey(c => c.EstId);
            builder.HasOne(c => c.FormaPagto).WithMany(c => c.ListaOrdemPagos).HasForeignKey(c => c.FopId);
        }
    }
}

