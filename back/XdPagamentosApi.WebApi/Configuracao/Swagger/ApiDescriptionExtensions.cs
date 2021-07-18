using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Configuracao.Swagger
{
    public static class ApiDescriptionExtensions
    {
        public static string GroupBySwaggerGroupAttribute(this ApiDescription api)
        {
            var actionDescriptor = api.GetProperty<ControllerActionDescriptor>();
            if (actionDescriptor == null)
            {
                actionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                api.SetProperty(actionDescriptor);
            }
            var customAttibutes = actionDescriptor?.MethodInfo.CustomAttributes?.ToList();
            var actionGroupNameAttribute = customAttibutes?.FirstOrDefault(p => p.AttributeType == typeof(SwaggerGroupAttribute));
            var sg = actionGroupNameAttribute?.ConstructorArguments?.FirstOrDefault() != null ? actionGroupNameAttribute.ConstructorArguments.FirstOrDefault().Value : string.Empty;
            return !string.IsNullOrEmpty(sg?.ToString()) ? sg.ToString() : actionDescriptor?.ActionName;
        }
    }
}
