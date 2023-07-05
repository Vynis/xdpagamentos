using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Helpers;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FluxoCaixaController : BaseController
    {
        private readonly IFluxoCaixaService _fluxoCaixaService;

        public FluxoCaixaController(IFluxoCaixaService fluxoCaixaService)
        {
            _fluxoCaixaService = fluxoCaixaService;
        }


        [HttpGet("buscar-contas/{conta}/{id}")]
        [SwaggerGroup("FluxoCaixa")]
        public async Task<IActionResult> BuscarContas(string conta, int id)
        {
            try
            {
                var response = new List<FluxoCaixa>();

                if (conta.Equals("CP"))
                {
                    return Response(await _fluxoCaixaService.BuscarExpressao(x => x.CpaId == id));
                }

                if (conta.Equals("CR"))
                {
                    return Response(await _fluxoCaixaService.BuscarExpressao(x => x.CorId == id));
                }

                return Response(response);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("inserir/{tipo}")]
        [SwaggerGroup("FluxoCaixa")]
        public async Task<IActionResult> Inserir(FluxoCaixa model,string tipo)
        {
            try
            {
                model.DtCadastro = DateTime.Now;
                model.Valor = model.Valor.RemoveWhiteSpaces();

                var response = await _fluxoCaixaService.AdicionarComBaixa(model,tipo);

                if (!response)
                    return Response("Erro ao cadastrar", false);

                return Response("Cadastro com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpGet("restaurar/{conta}/{id}")]
        [SwaggerGroup("FluxoCaixa")]
        public async Task<IActionResult> Restaurar(string conta, int id)
        {
            try
            {
                var response = await _fluxoCaixaService.Restaurar(id, conta);

                if (!response)
                    return Response("Erro ao cadastrar", false);

                return Response("Cadastro com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }
    }
}
