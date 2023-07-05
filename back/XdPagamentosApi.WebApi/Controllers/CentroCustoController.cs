using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CentroCustoController : BaseController
    {
        private readonly ICentroCustoService _centroCustoService;

        public CentroCustoController(ICentroCustoService centroCustoService)
        {
            _centroCustoService = centroCustoService;
        }

        [HttpGet("buscar-por-ativos")]
        [SwaggerGroup("CentroCusto")]
        public async Task<IActionResult> BuscarPorAtivos()
        {
            try
            {
                var response = await _centroCustoService.BuscarExpressao(x => x.Status.Equals("A"));

                return Response(response.ToList().OrderBy(c => c.Descricao));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-todos")]
        [SwaggerGroup("CentroCusto")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var response = await _centroCustoService.ObterTodos();

                return Response(response.ToList().OrderBy(c => c.Descricao));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("inserir")]
        [SwaggerGroup("CentroCusto")]
        public async Task<IActionResult> Inserir(CentroCusto model)
        {
            try
            {
                var response = await _centroCustoService.Adicionar(model);

                if (!response)
                    return Response("Erro ao cadastrar", false);

                return Response("Cadastro com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpPut("alterar")]
        [SwaggerGroup("CentroCusto")]
        public async Task<IActionResult> Alterar(CentroCusto model)
        {
            try
            {

                var response = await _centroCustoService.Atualizar(model);

                if (!response)
                    return Response("Erro ao alterar", false);

                return Response("Alteração com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpDelete("deletar/{id}")]
        [SwaggerGroup("CentroCusto")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var response = await _centroCustoService.ExcluirComValidacao(id);

                if (response.Count() > 0)
                    return Response(response, false);

                return Response("Exclusão com sucesso!");
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("CentroCusto")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                return Response(await _centroCustoService.ObterPorId(id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
