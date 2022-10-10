using System.Collections.Generic;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoGestaoPagamentoPorCliente
    {
        public string NomeCliente { get; set; }
        public string VlBrutoTotal { get; set; }
        public string VlTxPagSeguroTotal { get; set; }
        public string VlTxClienteTotal { get; set; }
        public string VlLiqOpeTotal { get; set; }
        public string VlPagtoTotal { get; set; }
        public string VlLucroTotal { get; set; }
        public List<DtoVwGestaoPagamentoTransacoes> ListaGestaoPagamento { get; set; } = new List<DtoVwGestaoPagamentoTransacoes>();
    }
}
