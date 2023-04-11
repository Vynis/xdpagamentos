using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciaTransacaoController : BaseController
    {
        private readonly IVwTransacoesSemOrdemPagtoService _vwTransacoesSemOrdemPagtoService;
        private readonly IMapper _mapper;

        public TransferenciaTransacaoController(IVwTransacoesSemOrdemPagtoService vwTransacoesSemOrdemPagtoService, IMapper mapper)
        {
            _vwTransacoesSemOrdemPagtoService = vwTransacoesSemOrdemPagtoService;
            _mapper = mapper;
        }

        [HttpPost("buscar-transacoes-sem-ordem-pagto")]
        [SwaggerGroup("TransferenciaTransacao")]
        public async Task<IActionResult> BuscarTransacoesSemOrdemPagto(PaginationFilter filtro)
        {
            try
            {
                var response = await _vwTransacoesSemOrdemPagtoService.ListaTransacoesSemOrdemPagtoTerminal(filtro);

                return Response(_mapper.Map<DtoTransacoesSemOrdemPagtoPorTerminal[]>(response.ToList()));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("gerar-ordem-pagto")]
        [SwaggerGroup("TransferenciaTransacao")]
        public async Task<IActionResult> GerarOrdemPagto(DtoParamOrdemPagto parametro)
        {
            try
            {
                var response = await _vwTransacoesSemOrdemPagtoService.Gerar(_mapper.Map<ParamOrdemPagto>(parametro));

                return Response("Ordem pagamento gerado!");
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
