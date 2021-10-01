using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Class;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Dtos;
using XdPagamentosApi.WebApi.Shared;
using XdPagamentosApi.WebApi.Shared.Extensions;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet("buscar-dados-usuario")]
        [SwaggerGroup("Usuario")]
        public async Task<IActionResult> BuscarUsuario()
        {
            try
            {
                var response = await _usuarioService.BuscarExpressao(x => x.Id == Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));

                if (!response.Any())
                    return Response("Usuario não encontrado", false);

                return Ok(new { name = response.FirstOrDefault().Nome, picture = "" });

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-usuarios-filtro")]
        [SwaggerGroup("Usuario")]
        public async Task<IActionResult> BuscarFiltro(string nome = "")
        {
            try
            {
                if (string.IsNullOrEmpty(nome))
                    return Response(await _usuarioService.ObterTodos());

                return Response(await _usuarioService.BuscarExpressao(x => x.Nome.Contains(nome)));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("Usuario")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                return Response(await _usuarioService.ObterPorId(id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("inserir")]
        [SwaggerGroup("Usuario")]
        public async Task<IActionResult> Inserir(DtoUsuario dtoUsuario)
        {
            try
            {
                //valida cpf
                var validacaoCpf = await _usuarioService.BuscarExpressao(x => x.CPF.Equals(dtoUsuario.CPF));

                if (validacaoCpf.Any())
                    return Response("Cpf já cadastrado", false);

                //valida email
                var validacaoEmail = await _usuarioService.BuscarExpressao(x => x.Email.Equals(dtoUsuario.Email));

                if (validacaoEmail.Any())
                    return Response("Email já cadastrado", false);


                dtoUsuario.Senha = SenhaHashService.CalculateMD5Hash(dtoUsuario.Senha.Trim());

                var response = await _usuarioService.Adicionar(_mapper.Map<Usuario>(dtoUsuario));

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
        [SwaggerGroup("Usuario")]
        public async Task<IActionResult> Alterar(DtoUsuario dtoUsuario)
        {
            try
            {
                var dados = await _usuarioService.ObterPorId(dtoUsuario.Id);

                if (!dados.CPF.Equals(dtoUsuario.CPF))
                {
                    //valida cpf
                    var validacaoCpf = await _usuarioService.BuscarExpressao(x => x.CPF.Equals(dtoUsuario.CPF));

                    if (validacaoCpf.Any())
                        return Response("Cpf já cadastrado", false);
                }

                if (!dados.Email.Equals(dtoUsuario.Email))
                {
                    //valida email
                    var validacaoEmail = await _usuarioService.BuscarExpressao(x => x.Email.Equals(dtoUsuario.Email));

                    if (validacaoEmail.Any())
                        return Response("Email já cadastrado", false);
                }

                var response = await _usuarioService.Atualizar(_mapper.Map<Usuario>(dtoUsuario));

                if (!response)
                    return Response("Erro ao alterar", false);

                return Response("Alteração com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpPut("alterar-senha")]
        [SwaggerGroup("Usuario")]
        public async Task<IActionResult> AlterarSenha(DtoAlteracaoSenhaUsuario dtoUsuario)
        {
            try
            {

                var codUsuarioLogado = User.Identity.Name.ToString();

                if (codUsuarioLogado != dtoUsuario.IdUsuario)
                    return Response("Erro usuario nao encontrado", false);

                var idUsuarioFormatado = Convert.ToInt32(dtoUsuario.IdUsuario.ToString().Descriptar());

                var usuario = await _usuarioService.BuscarExpressao(x => x.Id.Equals(idUsuarioFormatado) && x.Senha.Equals(SenhaHashService.CalculateMD5Hash(dtoUsuario.SenhaAtual)));

                if (!usuario.Any())
                    return Response("Senha atual invalida", false);

                var usuarioEncontrado = usuario.FirstOrDefault();  

                usuarioEncontrado.Senha = SenhaHashService.CalculateMD5Hash(dtoUsuario.SenhaNova);

                var response = await _usuarioService.Atualizar(usuarioEncontrado);

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
