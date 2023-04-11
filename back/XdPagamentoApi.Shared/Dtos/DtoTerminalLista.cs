using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoTerminalLista
    {
        public int Id { get; set; }
        public string NumTerminal { get; set; }
        public string Status { get; set; }
        public int EstId { get; set; }
        public DtoEstabelecimento Estabelecimento { get; set; }

        public List<DtoRelClienteTerminal> ListaRelClienteTerminal { get; set; }

        public string EstabelecimentoFormatado { 
            get
            {
                return Estabelecimento.Nome;
            }
        }

        public string ClienteFormatado { 
            get
            {
                return ListaRelClienteTerminal.Count() > 0 ? ListaRelClienteTerminal.FirstOrDefault().Cliente.Nome : "-";
            }
        }
    }
}
