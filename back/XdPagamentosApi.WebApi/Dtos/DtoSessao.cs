using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoSessao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Referencia { get; set; }

        public List<DtoPermissao> ListaPermissao { get; set; }
    }
}
