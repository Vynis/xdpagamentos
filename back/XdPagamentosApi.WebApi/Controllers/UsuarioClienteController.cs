using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentoApi.Shared.Enums;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Class;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.Shared.Extensions;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioClienteController : BaseController
    {
        private readonly IUsuarioClienteService _usuarioClienteService;
        private readonly IMapper _mapper;

        public UsuarioClienteController(IUsuarioClienteService usuarioClienteService, IMapper mapper)
        {
            _usuarioClienteService = usuarioClienteService;
            _mapper = mapper;
        }



        [HttpGet("buscar-todos-ativos")]
        [SwaggerGroup("UsuarioCliente")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var dados = await _usuarioClienteService.BuscarExpressao(x => x.Status.Equals("A"));

                return Response(dados.OrderBy(c => c.Nome));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-usuarios-filtro")]
        [SwaggerGroup("UsuarioCliente")]
        public async Task<IActionResult> BuscarFiltro(string nome = "")
        {
            try
            {
                if (string.IsNullOrEmpty(nome))
                    return Response(await _usuarioClienteService.ObterTodos());

                return Response(await _usuarioClienteService.BuscarExpressao(x => x.Nome.Contains(nome)));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("UsuarioCliente")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                return Response(await _usuarioClienteService.ObterPorId(id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpPost("inserir")]
        [SwaggerGroup("UsuarioCliente")]
        public async Task<IActionResult> Inserir(DtoUsuarioClienteLogado dtoUsuario)
        {
            try
            {

                //valida email
                var validacaoEmail = await _usuarioClienteService.BuscarExpressao(x => x.Email.Equals(dtoUsuario.Email));

                if (validacaoEmail.Any())
                    return Response("Email já cadastrado", false);


                dtoUsuario.Senha = SenhaHashService.CalculateMD5Hash(dtoUsuario.Senha.Trim());

                var response = await _usuarioClienteService.Adicionar(_mapper.Map<UsuarioCliente>(dtoUsuario));

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
        [SwaggerGroup("UsuarioCliente")]
        public async Task<IActionResult> Alterar(DtoUsuarioClienteLogado dtoUsuario)
        {
            try
            {
                var dados = await _usuarioClienteService.ObterPorId(dtoUsuario.Id);

                if (!dados.Email.Equals(dtoUsuario.Email))
                {
                    //valida email
                    var validacaoEmail = await _usuarioClienteService.BuscarExpressao(x => x.Email.Equals(dtoUsuario.Email));

                    if (validacaoEmail.Any())
                        return Response("Email já cadastrado", false);
                }

                var response = await _usuarioClienteService.Atualizar(_mapper.Map<UsuarioCliente>(dtoUsuario));

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
        [SwaggerGroup("UsuarioCliente")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {

                var dados = await _usuarioClienteService.ObterPorId(id);

                var response = await _usuarioClienteService.Excluir(dados);

                if (response == false)
                    return Response("Existe clientes vinculado com este usuario", false);

                return Response("Exclusão com sucesso!");
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }
    }
}
