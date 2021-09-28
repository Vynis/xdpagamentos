using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public List<Permissao> ListaPermissao { get; set; }
        public List<RelUsuarioEstabelecimento> ListaUsuarioEstabelecimentos { get; set; }
    }
}
