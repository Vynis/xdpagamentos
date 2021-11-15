using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoVwTransacoesSemOrdemPagto
    {
        private string _VlBrutoFormatado = "";
        private string _DataOperacaoFormatado = "";



        public int Id { get; set; }
        public DateTime? DataOperacao { get; set; }
        public string NumTerminal { get; set; }
        public string QtdParcelas { get; set; }
        public string CodTransacao { get; set; }
        public string VlBruto { get; set; }
        public DateTime? DataGravacao { get; set; }
        public string Estabelecimento { get; set; }
        public int? ClidId { get; set; }
        public string Cliente { get; set; }

        public string VlBrutoFormatado
        {
            get => $"R$ { Convert.ToDecimal(VlBruto) }";
            set { _VlBrutoFormatado = value; }
        }

        public string DataOperacaoFormatado
        {
            get => DataOperacao?.ToString("dd/MM/yyyy");
            set => _DataOperacaoFormatado = value;
        }

    }
}
