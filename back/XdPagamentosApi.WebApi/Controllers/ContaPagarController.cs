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
    public class ContaPagarController : BaseController
    {
        private readonly IContaPagarService _contaPagarService;

        public ContaPagarController(IContaPagarService contaPagarService)
        {
            _contaPagarService = contaPagarService;
        }

        [HttpGet("buscar-todos")]
        [SwaggerGroup("ContaPagar")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var response = await _contaPagarService.ObterTodos();

                return Response(response.ToList().OrderBy(c => c.Descricao));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("inserir")]
        [SwaggerGroup("ContaPagar")]
        public async Task<IActionResult> Inserir(ContaPagar model)
        {
            try
            {
                var response = await _contaPagarService.Adicionar(model);

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
        [SwaggerGroup("ContaPagar")]
        public async Task<IActionResult> Alterar(ContaPagar model)
        {
            try
            {

                var response = await _contaPagarService.Atualizar(model);

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
        [SwaggerGroup("ContaPagar")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                //var response = await _contaPagarService.Excluir(id);

                //if (response.Count() > 0)
                //    return Response(response, false);

                return Response("Exclusão com sucesso!");
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpPost("buscar-com-filtro")]
        [SwaggerGroup("ContaPagar")]
        public async Task<IActionResult> BuscarFiltro(PaginationFilter filtro)
        {
            try
            {
                return Response(await _contaPagarService.BuscarComFiltro(filtro));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
