using EnumsNET;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Enums;
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

        [HttpGet("buscar-por-ativos")]
        [SwaggerGroup("FormaPagto")]
        public async Task<IActionResult> BuscarPorAtivos()
        {
            try
            {
                var response = await _formaPagtoService.BuscarExpressao(x => x.Status.Equals("A"));

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
                var response = await _formaPagtoService.BuscarExpressao(x => x.Status.Equals("A"));

                var clienteLgado = await _clienteService.ObterPorId(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));

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

                return Response(response);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
    }
}
