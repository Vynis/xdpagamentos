using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Shared;
using XdPagamentosApi.WebApi.Shared.Extensions;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
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
    }
}
