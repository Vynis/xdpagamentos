using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoResponseRelatorioContaCorrente
    {
        public IEnumerable<VwRelatorioSaldoContaCorrente> lista { get; set; }
        public decimal EntradasTotal { get; set; }
        public decimal SaidasTotal { get; set; }
        public decimal SaldoFinalTotal { get; set; }
    }
}
