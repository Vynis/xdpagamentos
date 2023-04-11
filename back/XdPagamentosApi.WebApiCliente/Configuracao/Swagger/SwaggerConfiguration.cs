using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace XdPagamentosApi.WebApiCliente.Configuracao.Swagger
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "XD Pagamentos Api - Cliente",
                    Version = "v1",
                    Description = "API para fornecer serviços",
                    Contact = new OpenApiContact()
                    {
                        Email = "contato@xdpagamentos.com.br",
                        Name = "XD Pagaemntos API",
                        Url = new Uri("http://xdpagamentos.com.br")
                    }
                });
                c.TagActionsBy(api => api.GroupBySwaggerGroupAttribute());
            });


        }
        public static void ConfigSwaggerApp(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.DocExpansion(DocExpansion.None);
                s.SwaggerEndpoint("../swagger/v1/swagger.json", "XD Pagamentos API V1.0");
                s.DocumentTitle = "XD Pagamentos API";
            });
        }

    }
}
