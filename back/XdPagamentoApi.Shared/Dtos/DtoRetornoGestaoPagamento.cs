﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoRetornoGestaoPagamento
    {
        public DtoVwGestaoPagamentoTransacoes[] listaGestaoPagamentos { get; set; }

        public string SaldoAnterior { get; set; }
        public string Entradas { 
            get
            {
                return $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", listaGestaoPagamentos.Where(x => x.Tipo.Equals("C")).Sum(x => decimal.Parse(x.VlLiquidoCliente.Replace(".",""), new NumberFormatInfo() { NumberDecimalSeparator = "," })))} (C)";
            }
        }
        public string Saidas { 
            get
            {
                return $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", listaGestaoPagamentos.Where(x => x.Tipo.Equals("D")).Sum(x => decimal.Parse(x.VlLiquido.Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," })))} (D)";
            }
        }
        public string SaldoParcial { get; set; }
        public string SaldoAtual { get; set; }
    }
}
