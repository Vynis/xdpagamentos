using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoTerminal
    {
        public int Id { get; set; }
        public string NumTerminal { get; set; }
        public string Status { get; set; }
        public int EstId { get; set; }
        public DtoEstabelecimento Estabelecimento { get; set; }

        public List<DtoRelClienteTerminal> ListaRelClienteTerminal { get; set; }
    }
}
