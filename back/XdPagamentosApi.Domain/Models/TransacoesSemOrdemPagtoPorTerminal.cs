using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class TransacoesSemOrdemPagtoPorTerminal
    {
        public string NumTerminal { get; set; }
        public List<VwTransacoesSemOrdemPagto> ListaTransacoes { get; set; }
    }
}
