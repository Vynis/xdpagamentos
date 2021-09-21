using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class Sessao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Referencia { get; set; }

        public List<Permissao> ListaPermissao { get; set; }
    }
}
