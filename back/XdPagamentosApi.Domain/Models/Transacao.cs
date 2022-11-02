using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class Transacao 
    {
        public int Id { get; set; }
        public DateTime DtOperacao { get; set; }
        public string VlBruto { get; set; }
        public string NumEstabelecimento { get; set; }
        public string NumTerminal { get; set; }
        public string NumCartao { get; set; }
        public string QtdParcelas { get; set; }
        public string Chave { get; set; }
        public string CodAutorizacao { get; set; }
        public string TitPercDesconto { get; set; }
        public int PagId { get; set; }
        public Pagamentos Pagamentos { get; set; }
        public int HisId { get; set; }
        public int CliId { get; set; }
        public Cliente Cliente { get; set; }
        public int OpeId { get; set; }
        public DateTime? DtGravacao { get; set; }
        public string TipoTransacao { get; set; }
        public string OrigemAjuste { get; set; }
        public string MeioCaptura { get; set; }
        public string TaxaComissaoOperador { get; set; }
        public string Descricao { get; set; }
        public string VlLiquido { get; set; }
        public string VlTxAdm { get; set; }
        public string VlTxAdmPercentual { get; set; }
        public DateTime DtCredito { get; set; }
        public int EstId { get; set; }
        public string Status { get; set; }
        public int StatusCodigo { get; set; }

        [NotMapped]
        public string VlBrutoFormatado { get; set; }
        [NotMapped]
        public string VlLiquidoFormatado { get; set; }

    }
}
