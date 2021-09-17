﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Dtos;
using XdPagamentosApi.WebApi.Shared;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCaixaController : BaseController
    {
        private readonly IContaCaixaService _contaCaixaService;
        private readonly IMapper _mapper;

        public ContaCaixaController(IContaCaixaService contaCaixaService, IMapper mapper)
        {
            _contaCaixaService = contaCaixaService;
            _mapper = mapper;
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
    }
}