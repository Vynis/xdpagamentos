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
using XdPagamentosApi.Services.Class;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.Shared.Extensions;
using XdPagamentosApi.WebApiCliente.Configuracao.Swagger;

namespace XdPagamentosApi.WebApiCliente.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : BaseController
    {
        private readonly IClienteService _clienteService;
        private readonly ITipoTransacaoService _tipoTransacaoService;
        private readonly IMapper _mapper;

        public ClienteController(IClienteService clienteService, ITipoTransacaoService tipoTransacaoService, IMapper mapper)
        {
            _clienteService = clienteService;
            _tipoTransacaoService = tipoTransacaoService;
            _mapper = mapper;
        }

        [HttpGet("buscar-por-ativos")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarPorAtivos()
        {
            try
            {

                var usuarioLogado = Convert.ToInt32(User.Identity.Name.ToString().Descriptar(tipoSistema: TipoSistema.Cliente));

                var response = await _clienteService.BuscarExpressao(x => x.Status.Equals("A") && x.UscId == usuarioLogado ) ;

                return Response(response.ToList().OrderBy(c => c.Nome));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }



        [HttpGet("buscar-dados-cliente-logado/{id}")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarDadosClienteLogado(int id)
        {
            try
            {
                var response = await _clienteService.ObterPorId(id);

                return Response(response);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpPut("atualizar-dados-bancarios-cliente-logado")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> AtualizarDadosBancariosClienteLogado(DtoCliente dtoCliente)
        {
            try
            {

                var clienteLgado = await _clienteService.ObterPorId(dtoCliente.Id);

                clienteLgado.BanId = dtoCliente.BanId;
                clienteLgado.NumAgencia = dtoCliente.NumAgencia;
                clienteLgado.NumConta = dtoCliente.NumConta;
                clienteLgado.TipoConta = dtoCliente.TipoConta;
                clienteLgado.TipoChavePix = dtoCliente.TipoChavePix;
                clienteLgado.ChavePix = dtoCliente.ChavePix;

                var response = await _clienteService.Atualizar(clienteLgado);

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
