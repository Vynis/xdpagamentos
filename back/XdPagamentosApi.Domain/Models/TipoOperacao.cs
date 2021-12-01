using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class TipoOperacao
    {
        public int Id { get; set; }
        public int OpeId { get; set; }
        public Operadora Operadora { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Ref { get; set; }
    }
}
