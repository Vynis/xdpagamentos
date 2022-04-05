using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class VwRelatorioSaldoCliente
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string CnpjCpf { get; set; }
        public string Limite { get; set; }
        public string SaldoAtual { get; set; }
        public string SaldoFinal { get; set; }
    }
}
