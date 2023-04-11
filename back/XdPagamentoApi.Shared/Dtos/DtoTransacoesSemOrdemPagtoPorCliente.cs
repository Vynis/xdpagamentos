using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoTransacoesSemOrdemPagtoPorCliente
    {
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string NumEstabelecimento { get; set; }
        public string NomeEstabelcimento { get; set; }
        

        public string VlBrutoTotal
        {
            get
            {
                decimal soma = 0;
                ListaTransacoes.ForEach(x => soma += Convert.ToDecimal(x.VlBruto));
                return $"R$ {soma}";
            }
        }

        public string VlTxAdminTotal
        {
            get
            {
                decimal soma = 0;
                ListaTransacoes.ForEach(x => soma += Convert.ToDecimal(x.VlTxAdmin));
                return $"R$ {soma}";
            }
        }

        public string VlLiquidoTotal
        {
            get
            {
                decimal soma = 0;
                ListaTransacoes.ForEach(x => soma += Convert.ToDecimal(x.VlLiquido));
                return $"R$ {soma}";
            }
        }

        public int QtdOperacoes
        {
            get => ListaTransacoes.Count(); 
        }

        public List<DtoVwTransacoesSemOrdemPagto> ListaTransacoes { get; set; }
    }
}
