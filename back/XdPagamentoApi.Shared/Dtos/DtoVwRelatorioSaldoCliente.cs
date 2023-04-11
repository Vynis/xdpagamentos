using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoVwRelatorioSaldoCliente
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string CnpjCpf { get; set; }
        public string Limite { get; set; }
        public string SaldoAtual { get; set; }
        public string SaldoFinal { get; set; }
    }
}
