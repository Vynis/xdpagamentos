using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class FormaPagto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }

        public List<OrdemPagto> ListaOrdemPagos { get; set; }
        public List<GestaoPagamento> ListaGestaoPagamento { get; set; }
    }
}
