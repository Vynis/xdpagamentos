using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace XdPagamentoApi.Shared.Helpers
{
    public static class HelperFuncoes
    {
        public static string ValorMoedaBRString(string valor) => string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", FormataValorDecimal(valor.Trim()));

        public static string ValorMoedaBRDouble(double valor) => string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", valor);

        public static string ValorMoedaBRDecimal(decimal valor) => string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", valor);

        public static decimal FormataValorDecimal(string valor) => decimal.Parse(valor.Trim().Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," });

    }
}
