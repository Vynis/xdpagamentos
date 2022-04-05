using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Shared;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : BaseController
    {
        private readonly IRelatoriosService _relatoriosService;

        public RelatoriosController(IRelatoriosService relatoriosService)
        {
            _relatoriosService = relatoriosService;
        }


        [HttpPost("buscar-relatorio-solicitacao")]
        [SwaggerGroup("Relatorios")]
        public async Task<IActionResult> BuscaRelatorioSolicitacao(PaginationFilter filtro)
        {
            try
            {
                var response = await _relatoriosService.BuscaRelatorioSolicitacao(filtro);

                return Response(response);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
