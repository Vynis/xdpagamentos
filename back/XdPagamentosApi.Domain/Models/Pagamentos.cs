using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class Pagamentos
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Obs { get; set; }
        public int CliId { get; set; }
        public Cliente Cliente { get; set; }
        public int OrpId { get; set; }
        public OrdemPagto OrdemPagto { get; set; }

        public List<Transacao> ListaTransacoes { get; set; }
    }
}
