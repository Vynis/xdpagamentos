using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class RelContaEstabelecimento
    {
        public int Id { get; set; }
        public int CocId { get; set; }
        public ContaCaixa ContaCaixa { get; set; }
        public int EstId { get; set; }
        public Estabelecimento Estabelecimento { get; set; }
        public List<GestaoPagamento> ListaGestaoPagamento { get; set; }
    }
}
