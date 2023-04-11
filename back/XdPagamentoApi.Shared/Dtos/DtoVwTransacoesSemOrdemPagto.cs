using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoVwTransacoesSemOrdemPagto
    {


        public int Id { get; set; }
        public DateTime DataOperacao { get; set; }
        public string NumTerminal { get; set; }
        public string QtdParcelas { get; set; }
        public string CodTransacao { get; set; }
        public string VlBruto { get; set; }
        public DateTime? DataGravacao { get; set; }
        public string Estabelecimento { get; set; }
        public int? ClidId { get; set; }
        public string Cliente { get; set; }

        public string VlLiquido { get; set; }
        public string VlTxAdmin { get; set; }
        public string VlTxAdminPercentual { get; set; }

        public int EstId { get; set; }

        public string VlBrutoFormatado
        {
            get => $"R$ { string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", Convert.ToDecimal(VlBruto)) }";
        }

        public string VlLiquidoFormatado
        {
            get => $"R$ { string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", Convert.ToDecimal(VlLiquido)) }";
        }

        public string VlTaxaAdminFormatado
        {
            get => $"R$ { Convert.ToDecimal(VlTxAdmin) } ({VlTxAdminPercentual} %) ";
        }

        public string DataOperacaoFormatado
        {
            get => DataOperacao.ToString("dd/MM/yyyy");
        }


        public string DataGravacaoFormatado
        {
            get => DataGravacao?.ToString("dd/MM/yyyy");
        }
    }
}
