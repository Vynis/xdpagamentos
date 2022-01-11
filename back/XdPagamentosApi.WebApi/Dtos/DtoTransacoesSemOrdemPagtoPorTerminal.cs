using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoTransacoesSemOrdemPagtoPorTerminal
    {
        public string NumTerminal { get; set; }
        

        public string VlBrutoTotal
        {
            get
            {
                decimal soma = 0;
                ListaTransacoes.ForEach(x => soma += Convert.ToDecimal(x.VlBruto));
                return $"R$ { string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", soma) }";
            }
        }

        public string VlTxAdminTotal
        {
            get
            {
                decimal soma = 0;
                ListaTransacoes.ForEach(x => soma += Convert.ToDecimal(x.VlTxAdmin));
                return $"R$ { string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", soma) }";
            }
        }

        public string VlLiquidoTotal
        {
            get
            {
                decimal soma = 0;
                ListaTransacoes.ForEach(x => soma += Convert.ToDecimal(x.VlLiquido));
                return $"R$ { string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", soma)}";
            }
        }

        public int QtdOperacoes
        {
            get => ListaTransacoes.Count(); 
        }

        public List<DtoVwTransacoesSemOrdemPagto> ListaTransacoes { get; set; }
    }
}
