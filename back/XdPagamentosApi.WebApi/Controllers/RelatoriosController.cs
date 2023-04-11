using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentoApi.Shared.Helpers;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.Shared.Extensions;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : BaseController
    {
        private readonly IRelatoriosService _relatoriosService;
        private readonly IMapper _mapper;

        public RelatoriosController(IRelatoriosService relatoriosService, IMapper mapper)
        {
            _relatoriosService = relatoriosService;
            _mapper = mapper;
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
                var response = _mapper.Map<DtoVwRelatorioSaldoCliente[]>(await  _relatoriosService.BuscaRelatorioSaldoCliente(filtro));

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

        [HttpPost("buscar-relatorio-saldo-conta-corrente")]
        [SwaggerGroup("Relatorios")]
        public async Task<IActionResult> BuscaRelatorioSaldoContaCorrente(PaginationFilter filtro)
        {
            try
            {
                var response = _mapper.Map<DtoVwRelatorioSaldoContaCorrente[]>( await _relatoriosService.BuscaRelatorioSaldoContaCorrente(filtro));

                var dto = new DtoResponseRelatorioContaCorrente();

                dto.lista = response;
                dto.SaidasTotal = response.ToList().Sum(x => x.Saidas);
                dto.SaldoFinalTotal = response.ToList().Sum(x => x.SaldoFinal);
                dto.EntradasTotal = response.ToList().Sum(x => x.Entradas);

                return Response(dto);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpGet("buscar-grafico-vendas")]
        [SwaggerGroup("Relatorios")]
        public async Task<IActionResult> BuscaGraficoVendas()
        {
            try
            {
                var response = await _relatoriosService.BuscaGraficoVendas(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));

                return Response(response);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
