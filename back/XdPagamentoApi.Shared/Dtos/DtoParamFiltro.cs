using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoParamFiltro
    {

        public object Filter { get; set; }
        public string SortOrder { get; set; }
        public string SortField { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public DtoParamFiltro()
        {
            PageNumber = 0;
            PageSize = 10;
        }

    }
}
