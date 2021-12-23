using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class GestaoPagamentoMap : IEntityTypeConfiguration<GestaoPagamento>
    {
        public void Configure(EntityTypeBuilder<GestaoPagamento> builder)
        {
            builder.ToTable("tb_gestao_pagamentos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("gep_id");
            builder.Property(c => c.DtHrLancamento).HasColumnName("gep_dt_hr_lancamento");
            builder.Property(c => c.Descricao).HasColumnName("gep_descricao");
            builder.Property(c => c.Tipo).HasColumnName("gep_tipo");
            builder.Property(c => c.VlBruto).HasColumnName("gep_valor_bruto");
            builder.Property(c => c.VlLiquido).HasColumnName("gep_valor_liquido");
            builder.Property(c => c.CodRef).HasColumnName("gep_cod_ref");
            builder.Property(c => c.Obs).HasColumnName("gep_obs");
            builder.Property(c => c.CliId).HasColumnName("gep_cli_id");
            builder.Property(c => c.FopId).HasColumnName("gep_fop_id");
            builder.Property(c => c.RceId).HasColumnName("gep_rce_id");
            builder.Property(c => c.Grupo).HasColumnName("gep_grupo");
            builder.Property(c => c.UsuNome).HasColumnName("gep_usu_nome");
            builder.Property(c => c.UsuCpf).HasColumnName("gep_usu_cpf");
            builder.Property(c => c.DtHrAcaoUsuario).HasColumnName("gep_dt_hr_acao_usuario");
            builder.Property(c => c.DtHrCredito).HasColumnName("gep_dt_hr_credito");
            builder.Property(c => c.Status).HasColumnName("gep_status");
            builder.Property(c => c.ValorSolicitadoCliente).HasColumnName("gep_valor_solicitado_cliente");
            builder.Property(c => c.DtHrSolicitacoCliente).HasColumnName("gep_dt_hr_solicitado_cliente");


            builder.HasOne(c => c.Cliente).WithMany(c => c.ListaGestaoPagamento).HasForeignKey(c => c.CliId).IsRequired(false);
            builder.HasOne(c => c.FormaPagto).WithMany(c => c.ListaGestaoPagamento).HasForeignKey(c => c.FopId).IsRequired(false);
            builder.HasOne(c => c.RelContaEstabelecimento).WithMany(c => c.ListaGestaoPagamento).HasForeignKey(c => c.RceId).IsRequired(false);

        }
    }
}
