using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class GraficoVendas
    {
        public IEnumerable<string> ListaDatas { get; set; }

        public IEnumerable<int> ListaValores { get; set; }
    }
}
