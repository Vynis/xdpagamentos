﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessaoController : BaseController
    {
        private readonly ISessaoService _sessaoService;

        public SessaoController(ISessaoService sessaoService)
        {
            _sessaoService = sessaoService;
        }

        [HttpGet("buscar-todos")]
        [SwaggerGroup("Sessao")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var response = await _sessaoService.ObterTodos();

                return Response(response.ToList().OrderBy(c => c.Descricao));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
