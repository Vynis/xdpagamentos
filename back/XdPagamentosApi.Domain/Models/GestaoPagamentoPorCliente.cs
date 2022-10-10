using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class GestaoPagamentoPorCliente
    {
        public string NomeCliente { get; set; }
        public string VlBrutoTotal { get; set; }
        public string VlTxPagSeguroTotal { get; set; }
        public string VlTxClienteTotal { get; set; }
        public string VlLiqOpeTotal { get; set; }
        public string VlPagtoTotal { get; set; }
        public string VlLucroTotal { get; set; }
        public List<VwGestaoPagamentoTransacoes> ListaGestaoPagamento { get; set; } = new List<VwGestaoPagamentoTransacoes>();  
    }
}
