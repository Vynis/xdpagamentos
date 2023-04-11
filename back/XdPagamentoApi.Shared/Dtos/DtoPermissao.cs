using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoPermissao
    {
        public int Id { get; set; }
        public int SesId { get; set; }
        public DtoSessao Sessao { get; set; }
        public int UsuId { get; set; }
        [JsonIgnore]
        public DtoUsuario Usuario { get; set; }
    }
}
