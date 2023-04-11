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
    public class TipoTransacaoController : BaseController
    {
        private readonly ITipoTransacaoService _tipoTransacaoService;

        public TipoTransacaoController(ITipoTransacaoService tipoTransacaoService)
        {
            _tipoTransacaoService = tipoTransacaoService;
        }

        [HttpGet("buscar-padrao")]
        [SwaggerGroup("TipoTransacao")]
        public async Task<IActionResult> BuscarPadrao()
        {
            try
            {
                return Response(await _tipoTransacaoService.BuscarExpressao(x => x.CliId.Equals(0) && x.Status.Equals("A")));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
