using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Helpers;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Dtos;
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



        [HttpPost("buscar-relatorio-saldo-clientes")]
        [SwaggerGroup("Relatorios")]
        public async Task<IActionResult> BuscaRelatorioSaldoClientes(PaginationFilter filtro)
        {
            try
            {
                var response = await _relatoriosService.BuscaRelatorioSaldoCliente(filtro);

                var dto = new DtoResponseRelatorioSaldoClientes();

                dto.lista = response;
                dto.SaldoAtualTotal = HelperFuncoes.ValorMoedaBRDecimal(response.ToList().Sum(x => HelperFuncoes.FormataValorDecimal(x.SaldoAtual)));
                dto.SaldoFinalTotal = HelperFuncoes.ValorMoedaBRDecimal(response.ToList().Sum(x => HelperFuncoes.FormataValorDecimal(x.SaldoFinal)));
                dto.LimiteTotal = HelperFuncoes.ValorMoedaBRDecimal(response.ToList().Sum(x => HelperFuncoes.FormataValorDecimal(x.Limite)));

                return Response(dto);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
