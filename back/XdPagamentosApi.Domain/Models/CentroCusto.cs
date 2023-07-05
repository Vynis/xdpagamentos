using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class CentroCusto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public List<ContaPagar> ListContaPagar { get; set; }
        public List<ContaReceber> ListContaReceber { get; set; }
    }
}
