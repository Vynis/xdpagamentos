using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoRelUsuarioEstabelecimento
    {
        public int Id { get; set; }
        public int EstId { get; set; }
        public DtoEstabelecimento Estabelecimento { get; set; }
        public int UsuId { get; set; }
        public DtoUsuario Usuario { get; set; }
    }
}
