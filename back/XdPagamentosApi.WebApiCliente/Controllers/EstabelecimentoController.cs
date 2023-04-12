using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.WebApiCliente.Configuracao.Swagger;

namespace XdPagamentosApi.WebApiCliente.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstabelecimentoController : BaseController
    {
        private readonly IEstabelecimentoService _estabelecimentoService;
        private readonly IMapper _mapper;

        public EstabelecimentoController(IEstabelecimentoService estabelecimentoService, IMapper mapper)
        {
            _estabelecimentoService = estabelecimentoService;
            _mapper = mapper;
        }

        [HttpGet("buscar-por-ativos")]
        [SwaggerGroup("Estabelecimento")]
        public async Task<IActionResult> BuscarPorAtivos()
        {
            try
            {
                var response = await _estabelecimentoService.BuscarExpressao(x => x.Status.Equals("A"));

                return Response(response.ToList().OrderBy(c => c.Nome));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

    }
}
