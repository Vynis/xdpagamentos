using FiltrDinamico.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class PaginationFilter
    {
        public IEnumerable<FiltroItem> Filtro { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
