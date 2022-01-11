using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class ParamOrdemPagto
    {
        public int IdConta { get; set; }
        public int IdCliente { get; set; }
        public DateTime DataLancamentoCredito { get; set; }
        public List<TransacoesSemOrdemPagtoPorCliente> ClientesSelecionados { get; set; }
        public List<TransacoesSemOrdemPagtoPorTerminal> TerminaisSelecionados { get; set; }
    }
}
