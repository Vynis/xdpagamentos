using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class RelClienteTerminal
    {
        public int Id { get; set; }
        public int CliId { get; set; }
        public Cliente Cliente { get; set; }
        public int TerId { get; set; }
        public Terminal Terminal { get; set; }
    }
}
