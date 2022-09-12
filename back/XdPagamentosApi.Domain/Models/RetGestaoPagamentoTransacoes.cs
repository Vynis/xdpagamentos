using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class RetGestaoPagamentoTransacoes
    {
        public VwGestaoPagamentoTransacoes[] listaGestaoPagamentos { get; set; }
        public string SaldoAnterior { get; set; }
        public string SaldoParcial { get; set; }
        public string SaldoAtual { get; set; }
    }
}
