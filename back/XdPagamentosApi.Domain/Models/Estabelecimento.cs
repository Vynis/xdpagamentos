using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class Estabelecimento
    {
        public int Id { get; set; }

        public List<Cliente> ListaClientes { get; set; }
    }
}
