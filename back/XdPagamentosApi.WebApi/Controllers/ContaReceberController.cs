﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
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
    [Route("[controller]")]
    public class ContaReceberController : BaseController
    {
        private readonly IContaReceberService _contaReceberService;
        private readonly IMapper _mapper;

        public ContaReceberController(IContaReceberService contaReceberService, IMapper mapper)
        {
            _contaReceberService = contaReceberService;
            _mapper = mapper;
        }

        [HttpGet("buscar-todos")]
        [SwaggerGroup("ContaReceber")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var response = await _contaReceberService.ObterTodos();

                return Response(response.ToList().OrderBy(c => c.Descricao));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("ContaReceber")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var response = await _contaReceberService.ObterPorId(id);

                return Response(response);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("inserir")]
        [SwaggerGroup("ContaReceber")]
        public async Task<IActionResult> Inserir(ContaReceber model)
        {
            try
            {
                model.Status = "NP";
                model.DataCadastro = DateTime.Now;

                var response = await _contaReceberService.Adicionar(model);

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
        [SwaggerGroup("ContaReceber")]
        public async Task<IActionResult> Alterar(ContaReceber model)
        {
            try
            {
                var modelOld = await _contaReceberService.ObterPorId(model.Id);

                model.Status = modelOld.Status;
                model.DataCadastro = modelOld.DataCadastro;

                var response = await _contaReceberService.Atualizar(model);

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
        [SwaggerGroup("ContaReceber")]
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
        [SwaggerGroup("ContaReceber")]
        public async Task<IActionResult> BuscarFiltro(PaginationFilter filtro)
        {
            try
            {
                return Response(_mapper.Map<DtoContaReceber[]>(await _contaReceberService.BuscarComFiltro(filtro)));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

    }
}
