using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoRetornoGestaoExtrato
    {
        public DtoGestaoPagamento[] listaGestaoPagamentos { get; set; }

        public string SaldoAnterior { get; set; }
        public string Entradas { 
            get
            {
                return $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", listaGestaoPagamentos.Where(x => x.Tipo.Equals("C")).Sum(x => decimal.Parse(x.VlBruto.Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," })).ToString(CultureInfo.GetCultureInfo("pt-BR")))} (C)";
            }
        }
        public string Saidas { 
            get
            {
                return $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", listaGestaoPagamentos.Where(x => x.Tipo.Equals("D")).Sum(x => decimal.Parse(x.VlBruto.Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," })).ToString(CultureInfo.GetCultureInfo("pt-BR")))} (D)";
            }
        }
        public string SaldoParcial { get; set; }
        public string SaldoAtual { get; set; }
    }
}
