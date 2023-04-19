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
using XdPagamentosApi.WebApiCliente.Configuracao.Swagger;

namespace XdPagamentosApi.WebApiCliente.Controllers
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


        [HttpGet("buscar-dados-usuario-cliente")]
        [SwaggerGroup("UsuarioCliente")]
        public async Task<IActionResult> BuscarUsuario()
        {
            try
            {

                var usuarioLogado = Convert.ToInt32(User.Identity.Name.ToString().Descriptar(tipoSistema: TipoSistema.Cliente));

                var response = await _usuarioClienteService.BuscarExpressao(x => x.Id == usuarioLogado && x.Status.Equals("A"));

                if (!response.Any())
                    return Response("Usuario não encontrado", false);

                return Ok(new { name = response.FirstOrDefault().Nome, picture = "" });

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }


        [HttpPut("alterar-senha")]
        [SwaggerGroup("UsuarioCliente")]
        public async Task<IActionResult> AlterarSenha(DtoAlteracaoSenhaUsuario dtoUsuario)
        {
            try
            {

                var codUsuarioLogado = User.Identity.Name.ToString();

                if (codUsuarioLogado != dtoUsuario.IdUsuario)
                    return Response("Erro usuario nao encontrado", false);

                var idUsuarioFormatado = Convert.ToInt32(dtoUsuario.IdUsuario.ToString().Descriptar( TipoSistema.Cliente ));

                var usuario = await _usuarioClienteService.BuscarExpressao(x => x.Id.Equals(idUsuarioFormatado) && x.Senha.Equals(SenhaHashService.CalculateMD5Hash(dtoUsuario.SenhaAtual)));

                if (!usuario.Any())
                    return Response("Senha atual invalida", false);

                var usuarioEncontrado = usuario.FirstOrDefault();

                usuarioEncontrado.Senha = SenhaHashService.CalculateMD5Hash(dtoUsuario.SenhaNova);

                var response = await _usuarioClienteService.Atualizar(usuarioEncontrado);

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
