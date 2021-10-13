using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class TipoTransacao
    {
        public int Id { get; set; }
        public string QtdParcelas { get; set; }
        public string PercDesconto { get; set; }
        public string Status { get; set; }
        public int CliId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
