﻿using EnumsNET;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Enums;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Shared;
using XdPagamentosApi.WebApi.Shared.Extensions;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormaPagtoController : BaseController
    {
        private readonly IFormaPagtoService _formaPagtoService;
        private readonly IClienteService _clienteService;

        public FormaPagtoController(IFormaPagtoService formaPagtoService, IClienteService clienteService)
        {
            _formaPagtoService = formaPagtoService;
            _clienteService = clienteService;
        }

        [HttpGet("buscar-por-ativos/{cliId}")]
        [SwaggerGroup("FormaPagto")]
        public async Task<IActionResult> BuscarPorAtivos(int cliId)
        {
            try
            {

                var clienteLgado = await _clienteService.ObterPorId(cliId);

                IEnumerable<FormaPagto> response = await FormatarFormaPagamento(clienteLgado);

                return Response(response.ToList().OrderBy(c => c.Descricao));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpGet("buscar-por-ativos-cliente")]
        [SwaggerGroup("FormaPagto")]
        public async Task<IActionResult> BuscarPorAtivosCliente()
        {
            try
            {

                var clienteLgado = await _clienteService.ObterPorId(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));

                IEnumerable<FormaPagto> response = await FormatarFormaPagamento(clienteLgado);

                return Response(response);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        private async Task<IEnumerable<FormaPagto>> FormatarFormaPagamento(Cliente clienteLgado)
        {
            var response = await _formaPagtoService.BuscarExpressao(x => x.Status.Equals("A"));

            if (clienteLgado == null)
                return response;

            response.ToList().OrderBy(c => c.Descricao).ToList().ForEach(x =>
            {
                var tipoContaTxt = clienteLgado.TipoConta == "P" ? "Poupança" : "Conta Corrente";

                switch (x.Id)
                {
                    case 2:
                        x.Descricao = clienteLgado.BanId == 0 ? x.Descricao : $"{x.Descricao} ({tipoContaTxt}): Banco: {clienteLgado.Banco.Numero} - {clienteLgado.Banco.Nome} | Ag: {clienteLgado.NumAgencia} | Conta: {clienteLgado.NumConta}";
                        break;
                    case 5:
                        x.Descricao = clienteLgado.TipoChavePix == null ? x.Descricao : $"{x.Descricao} ({((TiposChavePix)clienteLgado.TipoChavePix).AsString(EnumFormat.Description)}): {clienteLgado.ChavePix} ";
                        break;
                    default:
                        break;
                }
            });
            return response;
        }
    }
}
