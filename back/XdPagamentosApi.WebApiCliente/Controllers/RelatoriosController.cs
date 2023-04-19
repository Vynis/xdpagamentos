using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentoApi.Shared.Enums;
using XdPagamentoApi.Shared.Helpers;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.Shared.Extensions;
using XdPagamentosApi.WebApiCliente.Configuracao.Swagger;

namespace XdPagamentosApi.WebApiCliente.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatoriosController : BaseController
    {
        private readonly IRelatoriosService _relatoriosService;
        private readonly IMapper _mapper;

        public RelatoriosController(IRelatoriosService relatoriosService, IMapper mapper)
        {
            _relatoriosService = relatoriosService;
            _mapper = mapper;
        }

        [HttpGet("buscar-grafico-vendas/{id}")]
        [SwaggerGroup("Relatorios")]
        public async Task<IActionResult> BuscaGraficoVendas(int id)
        {
            try
            {
                var response = await _relatoriosService.BuscaGraficoVendas(id);

                return Response(response);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
