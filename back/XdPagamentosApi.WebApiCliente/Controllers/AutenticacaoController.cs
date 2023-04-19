using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentoApi.Shared.Enums;
using XdPagamentosApi.Services.Class;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.Shared.Extensions;
using XdPagamentosApi.Shared.Token;
using XdPagamentosApi.WebApiCliente.Configuracao.Swagger;

namespace XdPagamentosApi.WebApiCliente.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacaoController : BaseController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly IClienteService _clienteService;
        private readonly IUsuarioClienteService _usuarioClienteService;

        public AutenticacaoController(IUsuarioService usuarioService, IMapper mapper, IClienteService clienteService, IUsuarioClienteService usuarioClienteService)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
            _clienteService = clienteService;
            _usuarioClienteService = usuarioClienteService;
        }


        [HttpPost("AutenticarCliente")]
        [SwaggerGroup("AutenticacaoSistema")]
        [AllowAnonymous]
        public async Task<IActionResult> AutenticarCliente(DtoParamLoginUsuario param)
        {
            try
            {
                param.Senha = SenhaHashService.CalculateMD5Hash(param.Senha.Trim());

                var resposta = await _usuarioClienteService.BuscarExpressao(x => x.Email.Trim().ToUpper().Equals(param.Email.Trim().ToUpper())
                                                                    && x.Senha.Equals(param.Senha) && x.Status.Equals("A"));


                var usuario = _mapper.Map<DtoUsuarioClienteLogado>(resposta.FirstOrDefault());
                
                if (usuario == null)
                    return BadRequest("Usuário ou senha incorreto!");

                usuario.Tipo = "SistemaCliente";

                var token = TokenService.GenerateToken(usuario);

                return Response(new { usuario, token });
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }


    }
}
