using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanoContaController  : BaseController
    {
        private readonly IPlanoContaService _planoContaService;

        public PlanoContaController(IPlanoContaService planoContaService)
        {
            _planoContaService = planoContaService;
        }

        [HttpGet("buscar-por-ativos/{tipo}")]
        [SwaggerGroup("PlanoConta")]
        public async Task<IActionResult> BuscarPorAtivos(string tipo)
        {
            try
            {
                var response = await _planoContaService.BuscarExpressao(x => x.Status.Equals("A") && x.Tipo.Equals(tipo));

                return Response(response.ToList().OrderBy(c => c.Referencia));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
