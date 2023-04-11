using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoRelClienteTerminal
    {
        public int Id { get; set; }
        public int CliId { get; set; }
        public DtoCliente Cliente { get; set; }
        public int TerId { get; set; }
        public DtoTerminal Terminal { get; set; }
    }
}
