using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperadoraController : BaseController
    {
        private readonly IOperadoraService _operadoraService;

        public OperadoraController(IOperadoraService operadoraService)
        {
            _operadoraService = operadoraService;
        }

        [HttpGet("buscar-por-ativos")]
        [SwaggerGroup("Operadora")]
        public async Task<IActionResult> BuscarPorAtivos()
        {
            try
            {
                var response = await _operadoraService.BuscarExpressao(x => x.Status.Equals("A"));

                return Response(response.ToList().OrderBy(c => c.Nome));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
