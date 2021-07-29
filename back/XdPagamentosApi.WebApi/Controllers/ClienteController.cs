using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Shared;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : BaseController
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(IClienteService clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        [HttpPost("buscar-cliente-filtro")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarFiltro(PaginationFilter filtro)
        {
            try
            {
                return Response(await _clienteService.BuscarComFiltro(filtro));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                return Response(await _clienteService.ObterPorId(id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

    }
}
