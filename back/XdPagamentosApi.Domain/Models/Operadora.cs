using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class Operadora
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }

        public List<Estabelecimento> ListaEstabelecimento { get; set; }
    }
}
