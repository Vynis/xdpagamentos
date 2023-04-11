using System;
using System.Collections.Generic;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoResponseRelatorioSaldoClientes
    {
        public IEnumerable<DtoVwRelatorioSaldoCliente> lista { get; set; }
        public string SaldoAtualTotal { get; set; }
        public string LimiteTotal { get; set; }
        public string SaldoFinalTotal { get; set; }
    }
}
