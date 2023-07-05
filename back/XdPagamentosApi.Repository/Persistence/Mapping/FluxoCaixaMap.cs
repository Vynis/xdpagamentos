using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class FluxoCaixaMap : IEntityTypeConfiguration<FluxoCaixa>
    {
        public void Configure(EntityTypeBuilder<FluxoCaixa> builder)
        {
            builder.ToTable("tb_fluxo_caixa");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("flc_id");
            builder.Property(c => c.Descricao).HasColumnName("flc_descricao");
            builder.Property(c => c.Valor).HasColumnName("flc_valor");
            builder.Property(c => c.TipoPagamento).HasColumnName("flc_tipo_pagamento");
            builder.Property(c => c.CorId).HasColumnName("flc_cor_id");
            builder.Property(c => c.CpaId).HasColumnName("flc_cpa_id");
            builder.Property(c => c.PcoId).HasColumnName("flc_pco_id");
            builder.Property(c => c.CocId).HasColumnName("flc_coc_id");
            builder.Property(c => c.DtCadastro).HasColumnName("flc_dt_cadastro");
            builder.Property(c => c.DtLancamento).HasColumnName("flc_dt_lancamento");

            builder.HasOne(c => c.ContaReceber).WithMany(c => c.ListaFluxoCaixa).HasForeignKey(c => c.CorId).IsRequired(false);
            builder.HasOne(c => c.ContaPagar).WithMany(c => c.ListaFluxoCaixa).HasForeignKey(c => c.CpaId).IsRequired(false);
            builder.HasOne(c => c.PlanoConta).WithMany(c => c.ListaFluxoCaixa).HasForeignKey(c => c.PcoId);
            builder.HasOne(c => c.ContaCaixa).WithMany(c => c.ListaFluxoCaixa).HasForeignKey(c => c.CocId);

        }
    }
}
