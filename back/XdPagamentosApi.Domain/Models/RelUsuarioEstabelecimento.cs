using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class RelUsuarioEstabelecimento
    {
        public int Id { get; set; }
        public int EstId { get; set; }
        public Estabelecimento Estabelecimento { get; set; }
        public int UsuId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
