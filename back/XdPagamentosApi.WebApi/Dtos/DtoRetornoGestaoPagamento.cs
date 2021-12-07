using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoRetornoGestaoPagamento
    {
        public DtoGestaoPagamento[] listaGestaoPagamentos { get; set; }

        public string SaldoAnterior { get; set; }
        public string Entradas { 
            get
            {
                return $"{listaGestaoPagamentos.Where(x => x.Tipo.Equals("C")).Sum(x => Convert.ToDecimal(x.VlLiquido)).ToString()} (C)";
            }
        }
        public string Saidas { 
            get
            {
                return $"{listaGestaoPagamentos.Where(x => x.Tipo.Equals("D")).Sum(x => Convert.ToDecimal(x.VlLiquido)).ToString()} (D)";
            }
        }
        public string SaldoParcial { get; set; }
        public string SaldoAtual { get; set; }
    }
}
