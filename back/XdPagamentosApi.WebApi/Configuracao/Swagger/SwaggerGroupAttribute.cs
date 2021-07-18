using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Configuracao.Swagger
{
    public class SwaggerGroupAttribute : Attribute
    {
        public string GroupName { get; set; }
        public SwaggerGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}
