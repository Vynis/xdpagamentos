using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class PagamentosMap : IEntityTypeConfiguration<Pagamentos>
    {
        public void Configure(EntityTypeBuilder<Pagamentos> builder)
        {
            builder.ToTable("tb_pagamentos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("pag_id");
            builder.Property(c => c.Data).HasColumnName("pag_data");
            builder.Property(c => c.Obs).HasColumnName("pag_obs");
            builder.Property(c => c.CliId).HasColumnName("pag_cli_id");
            builder.Property(c => c.OrpId).HasColumnName("pag_orp_id");

            builder.HasOne(c => c.OrdemPagto).WithMany(c => c.ListaPagamentos).HasForeignKey(c => c.OrpId);
            builder.HasOne(c => c.Cliente).WithMany(c => c.ListaPagamentos).HasForeignKey(c => c.CliId);
        }
    }
}

