using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class VwGestaoPagamentoTransacoes
    {
        public int Id { get; set; }
        public DateTime DtHrLancamento { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string VlBruto { get; set; }
        public string VlLiquido { get; set; }
        public string CodRef { get; set; }
        public string Obs { get; set; }
        public int CliId { get; set; }
        public Cliente Cliente { get; set; }
        public int FopId { get; set; }
        public FormaPagto FormaPagto { get; set; }
        public int RceId { get; set; }
        public RelContaEstabelecimento RelContaEstabelecimento { get; set; }
        public string Grupo { get; set; }
        public string UsuNome { get; set; }
        public string UsuCpf { get; set; }
        public DateTime DtHrAcaoUsuario { get; set; }
        public DateTime DtHrCredito { get; set; }
        public string Status { get; set; }
        public string ValorSolicitadoCliente { get; set; }
        public DateTime? DtHrSolicitacoCliente { get; set; }
        public DateTime? DtAgendamento { get; set; }
        public string VlBrutoTransacao { get; set; }
        public string QtdParcelaTransacao { get; set; }
        public string CodAutorizacaoTransacao { get; set; }
        public string NumCartaoTransacao { get; set; }
        public string MeioCapturaTransacao { get; set; }
        public string TipoOperacaoTransacao { get; set; }
        public string ValorLiquidoOperadora { get; set; }
        public string ValorTaxaPagSeguro { get; set; }
        public string TaxaPagSeguro { get; set; }
        public string ValorTaxaPagCliente { get; set; }
        public string TaxaPagCliente { get; set; }
        public string VlLiquidoCliente { get; set; }
        public string NumTerminal { get; set; }
        public int EstId { get; set; }
        public string Estabelecimento { get; set; }
    }
}
