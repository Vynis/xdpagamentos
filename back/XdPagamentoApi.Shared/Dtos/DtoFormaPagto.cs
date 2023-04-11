using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoFormaPagto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
    }
}
