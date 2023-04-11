using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentoApi.Shared.Dtos
{
    public class DtoAlteracaoSenhaUsuario
    {
        [Required(ErrorMessage = "Informe o Id do Usuario")]
        public string IdUsuario { get; set; }
        [Required(ErrorMessage = "Informe a Senha Atual")]
        public string SenhaAtual { get; set; }
        [Required(ErrorMessage = "Informe a Senha Nova")]
        public string SenhaNova { get; set; }
    }
}
