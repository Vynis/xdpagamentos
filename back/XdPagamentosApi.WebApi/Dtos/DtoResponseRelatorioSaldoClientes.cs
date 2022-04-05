using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoResponseRelatorioSaldoClientes
    {
        public IEnumerable<VwRelatorioSaldoCliente> lista { get; set; }
        public string SaldoAtualTotal { get; set; }
        public string LimiteTotal { get; set; }
        public string SaldoFinalTotal { get; set; }
    }
}
