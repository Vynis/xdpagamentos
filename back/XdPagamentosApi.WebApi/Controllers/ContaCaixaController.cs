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
    public class ContaCaixaController : BaseController
    {
        private readonly IContaCaixaService _contaCaixaService;
        private readonly IMapper _mapper;
        private readonly IRelContaEstabelecimentoService _relContaEstabelecimentoService;

        public ContaCaixaController(IContaCaixaService contaCaixaService, IMapper mapper, IRelContaEstabelecimentoService relContaEstabelecimentoService)
        {
            _contaCaixaService = contaCaixaService;
            _mapper = mapper;
            _relContaEstabelecimentoService = relContaEstabelecimentoService;
        }

        [HttpGet("buscar-por-ativos")]
        [SwaggerGroup("ContaCaixa")]
        public async Task<IActionResult> BuscarPorAtivos()
        {
            try
            {
                var response = await _contaCaixaService.BuscarExpressao(x => x.Status.Equals("A"));

                return Response(response.ToList().OrderBy(c => c.Descricao));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpGet("buscar-todos")]
        [SwaggerGroup("ContaCaixa")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var response = await _contaCaixaService.ObterTodos();

                return Response(response.ToList().OrderBy(c => c.Descricao));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("ContaCaixa")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                return Response(await _contaCaixaService.ObterPorId(id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("inserir")]
        [SwaggerGroup("ContaCaixa")]
        public async Task<IActionResult> Inserir(DtoContaCaixa dto)
        {
            try
            {
                var response = await _contaCaixaService.Adicionar(_mapper.Map<ContaCaixa>(dto));

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
        [SwaggerGroup("ContaCaixa")]
        public async Task<IActionResult> Alterar(DtoContaCaixa dto)
        {
            try
            {

                var response = await _contaCaixaService.Atualizar(_mapper.Map<ContaCaixa>(dto));

                if (!response)
                    return Response("Erro ao alterar", false);

                return Response("Alteração com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-conta-caixa-estabelecimento")]
        [SwaggerGroup("ContaCaixa")]
        public async Task<IActionResult> BuscarTodosComEstabelecimento()
        {
            try
            {
                var response = await _relContaEstabelecimentoService.BuscarExpressao(x => x.Estabelecimento.Status.Equals("A"));

                return Response(response.ToList().OrderBy(c => c.Id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-conta-caixa-estabelecimento/{id}")]
        [SwaggerGroup("ContaCaixa")]
        public async Task<IActionResult> BuscarTodosComEstabelecimento(int id)
        {
            try
            {
                var response = await _relContaEstabelecimentoService.BuscarExpressao(x => x.Estabelecimento.Status.Equals("A") && x.CocId.Equals(id));

                return Response(response.ToList().OrderBy(c => c.Id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpDelete("deletar/{id}")]
        [SwaggerGroup("ContaCaixa")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {

                var response = await _contaCaixaService.ExcluirComValidacao(id);

                if (response.Count() > 0)
                    return Response(response, false);

                return Response("Exclusão com sucesso!");
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }
    }
}
