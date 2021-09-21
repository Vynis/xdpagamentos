using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class Permissao
    {
        public int Id { get; set; }
        public int SesId { get; set; }
        public Sessao Sessao { get; set; }
        public int UsuId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
