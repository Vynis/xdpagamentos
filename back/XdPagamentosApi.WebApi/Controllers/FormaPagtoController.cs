using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Shared;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormaPagtoController : BaseController
    {
        private readonly IFormaPagtoService _formaPagtoService;

        public FormaPagtoController(IFormaPagtoService formaPagtoService)
        {
            _formaPagtoService = formaPagtoService;
        }

        [HttpGet("buscar-por-ativos")]
        [SwaggerGroup("FormaPagto")]
        public async Task<IActionResult> BuscarPorAtivos()
        {
            try
            {
                var response = await _formaPagtoService.BuscarExpressao(x => x.Status.Equals("A"));

                return Response(response.ToList().OrderBy(c => c.Descricao));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
