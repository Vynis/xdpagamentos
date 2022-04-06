using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoAlteracaoSenhaCliente
    {
        [Required(ErrorMessage = "Informe o Id do Usuario")]
        public string IdCliente { get; set; }
        [Required(ErrorMessage = "Informe a Senha Atual")]
        public string SenhaAtual { get; set; }
        [Required(ErrorMessage = "Informe a Senha Nova")]
        public string SenhaNova { get; set; }
    }
}
