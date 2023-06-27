using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class ContaReceber
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public string Obs { get; set; }
        public DateTime DataCadastro { get; set; }
        public int CecId { get; set; }
        public CentroCusto CentroCusto { get; set; }
    }
}
