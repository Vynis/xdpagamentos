using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class VwTransacoesSemOrdemPagtoMap : IEntityTypeConfiguration<VwTransacoesSemOrdemPagto>
    {
        public void Configure(EntityTypeBuilder<VwTransacoesSemOrdemPagto> builder)
        {
            builder.ToTable("VW_TRANSACOES_SEM_ORDEM_PGTO");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("tra_id");
            builder.Property(c => c.DataOperacao).HasColumnName("tra_dt_operacao");
            builder.Property(c => c.NumTerminal).HasColumnName("tra_ter_num_terminal");
            builder.Property(c => c.QtdParcelas).HasColumnName("tra_qtd_parcelas");
            builder.Property(c => c.CodTransacao).HasColumnName("tra_cod_autorizacao");
            builder.Property(c => c.VlBruto).HasColumnName("tra_vl_bruto");
            builder.Property(c => c.DataGravacao).HasColumnName("tra_dt_gravacao");
            builder.Property(c => c.Estabelecimento).HasColumnName("est_nome");
            builder.Property(c => c.ClidId).HasColumnName("tra_cli_id");
            builder.Property(c => c.Cliente).HasColumnName("cli_nome");
            builder.Property(c => c.VlLiquido).HasColumnName("tra_vl_liquido"); 
            builder.Property(c => c.VlTxAdmin).HasColumnName("tra_tx_adm");
            builder.Property(c => c.VlTxAdminPercentual).HasColumnName("tra_tx_adm_percentual");
            builder.Property(c => c.EstId).HasColumnName("tra_est_id");     

        }
    }
}
