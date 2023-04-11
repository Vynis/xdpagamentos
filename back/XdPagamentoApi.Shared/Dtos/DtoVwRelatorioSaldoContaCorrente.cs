using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoVwRelatorioSaldoContaCorrente
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Entradas { get; set; }
        public decimal Saidas { get; set; }
        public decimal SaldoFinal { get; set; }
    }
}
