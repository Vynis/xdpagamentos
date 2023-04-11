using System.Collections.Generic;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoResponseRelatorioContaCorrente
    {
        public IEnumerable<DtoVwRelatorioSaldoContaCorrente> lista { get; set; }
        public decimal EntradasTotal { get; set; }
        public decimal SaidasTotal { get; set; }
        public decimal SaldoFinalTotal { get; set; }
    }
}
