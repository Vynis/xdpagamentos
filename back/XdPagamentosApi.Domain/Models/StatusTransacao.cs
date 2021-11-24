using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class StatusTransacao
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string DescStatus { get; set; }
        public string Tipo { get; set; }
    }
}
