using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class OrdemPagto
    {
        public int Id { get; set; }
        public int FopId { get; set; }
        public FormaPagto FormaPagto { get; set; }
        public string Valor { get; set; }
        public string NumIdentifcaocao { get; set; }
        public string Status { get; set; }
        public string Chave { get; set; }
        public DateTime? DtEmissao { get; set; }
        public DateTime? DtBaixa { get; set; }
        public int EstId { get; set; }
        public Estabelecimento Estabelecimento { get; set; }

        public List<OrdemPagto> ListaOrdempagto { get; set; }
        public List<Pagamentos> ListaPagamentos { get; set; }
    }
}
