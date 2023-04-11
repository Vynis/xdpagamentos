using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoUsuario
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Status { get; set; }

        public List<DtoPermissao> ListaPermissao { get; set; }
        public List<DtoRelUsuarioEstabelecimento> ListaUsuarioEstabelecimentos { get; set; }
    }
}
