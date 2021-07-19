using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Dtos
{
    public class DtoParamLoginUsuario
    {
        [Required]
        public string CPF { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
