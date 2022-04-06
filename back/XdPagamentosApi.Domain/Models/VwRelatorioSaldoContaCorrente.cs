using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class VwRelatorioSaldoContaCorrente
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Entradas { get; set; }
        public decimal Saidas { get; set; }
        public decimal SaldoFinal { get; set; }
    }
}
