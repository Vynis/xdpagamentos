using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Services.Class;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Configuracao.Token;
using XdPagamentosApi.WebApi.Dtos;
using XdPagamentosApi.WebApi.Shared;
using XdPagamentosApi.WebApi.Shared.Extensions;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : BaseController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public AutenticacaoController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpPost]
        [SwaggerGroup("AutenticacaoSistema")]
        [AllowAnonymous]
        public async Task<IActionResult> Autenticar(DtoParamLoginUsuario param)
        {
            try
            {
                param.Senha = SenhaHashService.CalculateMD5Hash(param.Senha.Trim());

                var resposta = await _usuarioService.BuscarExpressao(x => x.CPF.Trim().ToUpper().Equals(param.CPF.Trim().ToUpper())
                                                                    && x.Senha.Equals(param.Senha) && x.Status.Equals("A"));


                var usuario = _mapper.Map<DtoUsuarioLogado>(resposta.FirstOrDefault());

                if (usuario == null)
                    return Response("Usuário ou senha incorreto!", false);

                var token = TokenService.GenerateToken(usuario);

                return Response(new { usuario, token });
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }


        [HttpGet("permissao-usuario")]
        [SwaggerGroup("AutenticacaoSistema")]
        public async Task<IActionResult> PermissaoUsuario()
        {
            try
            {

                var resposta = await _usuarioService.ObterPorId(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));

                var usuario = _mapper.Map<DtoUsuario>(resposta);

                return Response(usuario.ListaPermissao);
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }


    }
}
