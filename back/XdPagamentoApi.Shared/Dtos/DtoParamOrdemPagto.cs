using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoParamOrdemPagto
    {
        public int IdConta { get; set; }
        public int IdCliente { get; set; }
        public DateTime DataLancamentoCredito { get; set; }
        public List<DtoTransacoesSemOrdemPagtoPorCliente> ClientesSelecionados { get; set; }
        public List<DtoTransacoesSemOrdemPagtoPorTerminal> TerminaisSelecionados { get; set; }
    }
}
