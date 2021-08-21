using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoContaCaixa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
    }
}
