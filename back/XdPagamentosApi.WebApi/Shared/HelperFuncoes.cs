using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Shared
{
    public static class HelperFuncoes
    {
        public static string ValorMoedaBR(string valor) => string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", decimal.Parse(valor.Replace(".",""), new NumberFormatInfo() { NumberDecimalSeparator = "," }));

    }
}
