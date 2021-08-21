using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class ContaCaixa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
    }
}
