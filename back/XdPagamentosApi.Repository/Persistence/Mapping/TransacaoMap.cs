using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class TransacaoMap : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.ToTable("tb_transacoes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("tra_id");
            builder.Property(c => c.DtOperacao).HasColumnName("tra_dt_operacao");
            builder.Property(c => c.VlBruto).HasColumnName("tra_vl_bruto");
            builder.Property(c => c.NumEstabelecimento).HasColumnName("tra_est_num_estabelecimento");
            builder.Property(c => c.NumTerminal).HasColumnName("tra_ter_num_terminal");
            builder.Property(c => c.NumCartao).HasColumnName("tra_num_cartao");
            builder.Property(c => c.QtdParcelas).HasColumnName("tra_qtd_parcelas");
            builder.Property(c => c.Chave).HasColumnName("tra_chave");
            builder.Property(c => c.CodAutorizacao).HasColumnName("tra_cod_autorizacao");
            builder.Property(c => c.TitPercDesconto).HasColumnName("tra_tit_perc_desconto");
            builder.Property(c => c.PagId).HasColumnName("tra_pag_id");
            builder.Property(c => c.HisId).HasColumnName("tra_his_id");
            builder.Property(c => c.CliId).HasColumnName("tra_cli_id");
            builder.Property(c => c.OpeId).HasColumnName("tra_ope_id");
            builder.Property(c => c.DtGravacao).HasColumnName("tra_dt_gravacao");
            builder.Property(c => c.TipoTransacao).HasColumnName("tra_tipo_transacao");
            builder.Property(c => c.OrigemAjuste).HasColumnName("tra_origem_ajuste");
            builder.Property(c => c.MeioCaptura).HasColumnName("tra_meio_captura");
            builder.Property(c => c.TaxaComissaoOperador).HasColumnName("tra_taxa_comissao_operador");
            builder.Property(c => c.Descricao).HasColumnName("tra_tio_descricao");
            builder.Property(c => c.VlLiquido).HasColumnName("tra_vl_liquido");
            builder.Property(c => c.VlTxAdm).HasColumnName("tra_tx_adm");
            builder.Property(c => c.VlTxAdmPercentual).HasColumnName("tra_tx_adm_percentual");
            builder.Property(c => c.DtCredito).HasColumnName("tra_dt_hr_credito");
            builder.Property(c => c.EstId).HasColumnName("tra_est_id");
            builder.Property(c => c.Status).HasColumnName("tra_status");

            builder.HasOne(c => c.Cliente).WithMany(c => c.ListaTransacoes).HasForeignKey(c => c.CliId);
            builder.HasOne(c => c.Pagamentos).WithMany(c => c.ListaTransacoes).HasForeignKey(c => c.PagId);
            

        }
    }
}

