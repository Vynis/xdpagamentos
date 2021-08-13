using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class Terminal
    {
        public int Id { get; set; }
        public string NumTerminal { get; set; }
        public string Status { get; set; }
        public int EstId { get; set; }
        public Estabelecimento Estabelecimento { get; set; }

        public List<RelClienteTerminal> ListaRelClienteTerminal { get; set; }
    }
}
