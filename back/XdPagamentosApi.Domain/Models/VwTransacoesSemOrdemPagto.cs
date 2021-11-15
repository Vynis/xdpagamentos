using System;
using System.Collections.Generic;
using System.Text;


namespace XdPagamentosApi.Domain.Models
{
    public class VwTransacoesSemOrdemPagto
    {
        public int Id { get; set; }
        public DateTime? DataOperacao { get; set; }
        public string NumTerminal { get; set; }
        public string QtdParcelas { get; set; }
        public string CodTransacao { get; set; }
        public string VlBruto { get; set; }
        public DateTime? DataGravacao { get; set; }
        public string Estabelecimento { get; set; }
        public int? ClidId { get; set; }
        public  string Cliente { get; set; }
    }
}
