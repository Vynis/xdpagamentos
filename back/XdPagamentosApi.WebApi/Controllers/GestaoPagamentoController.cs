﻿using AutoMapper;
using FiltrDinamico.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Dtos;
using XdPagamentosApi.WebApi.Shared;
using XdPagamentosApi.WebApi.Shared.Extensions;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GestaoPagamentoController : BaseController
    {
        private readonly IGestaoPagamentoService _gestaoPagamentoService;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        private readonly IClienteService _clienteService;

        public GestaoPagamentoController(IGestaoPagamentoService gestaoPagamentoService, IMapper mapper, IUsuarioService usuarioService, IClienteService clienteService)
        {
            _gestaoPagamentoService = gestaoPagamentoService;
            _mapper = mapper;
            _usuarioService = usuarioService;
            _clienteService = clienteService;
        }


        [HttpPost("buscar-gestao-pagamento-filtro")]
        [SwaggerGroup("GestaoPagamento")]
        public async Task<IActionResult> BuscarFiltro(PaginationFilter filtro)
        {
            try
            {

                if (filtro.Filtro.Count() == 0)
                    return Response("Selecione os filtros obrigatorios", false);

                if (!ValidaFiltro(filtro,"DtHrLancamento") || !ValidaFiltro(filtro, "CliId"))
                    return Response("Selecione os filtros obrigatorios", false);

                var retornoGestaoPagamento = new DtoRetornoGestaoPagamento();

                var listaFiltroPadrao = new List<FiltroItem>();
                listaFiltroPadrao.AddRange(filtro.Filtro);
                listaFiltroPadrao.Add(new FiltroItem { FilterType = "equals", Property = "Grupo", Value = "EC" });

                filtro.Filtro = listaFiltroPadrao;

                var listaPagamentos = _mapper.Map<DtoGestaoPagamento[]>( await _gestaoPagamentoService.BuscarComFiltro(filtro));

                retornoGestaoPagamento.listaGestaoPagamentos = listaPagamentos;

                if (listaPagamentos.Count() == 0)
                    return Response(retornoGestaoPagamento);

                var dadosCliente = listaPagamentos.FirstOrDefault().CliId;

                //Saldo Atual
                var dadosGeral = await _gestaoPagamentoService.BuscarExpressao(x => x.CliId.Equals(dadosCliente) && x.Grupo.Equals("EC"));

                retornoGestaoPagamento.SaldoAtual = (dadosGeral.Where(x => x.Tipo.Equals("C")).Sum(x => decimal.Parse(x.VlLiquido, new NumberFormatInfo() { NumberDecimalSeparator = "," })) - dadosGeral.Where(x => x.Tipo.Equals("D")).Sum(x => decimal.Parse(x.VlLiquido, new NumberFormatInfo() { NumberDecimalSeparator = "," }))).ToString();

                //Saldo Anterior
                var dataHrLancamento = filtro.Filtro.Where(x => x.Property.Equals("DtHrLancamento") && x.FilterType.Equals("greaterThanEquals")).FirstOrDefault().Value.ToString();

                var dadosSaldoAnterior = await _gestaoPagamentoService.BuscarExpressao(x => x.DtHrLancamento < DateTime.Parse(dataHrLancamento) && x.CliId.Equals(dadosCliente));

                retornoGestaoPagamento.SaldoAnterior = (dadosSaldoAnterior.Where(x => x.Tipo.Equals("C")).Sum(x => decimal.Parse(x.VlLiquido, new NumberFormatInfo() { NumberDecimalSeparator = "," })) - dadosSaldoAnterior.Where(x => x.Tipo.Equals("D")).Sum(x => decimal.Parse(x.VlLiquido, new NumberFormatInfo() { NumberDecimalSeparator = "," }))).ToString();

                return Response(retornoGestaoPagamento);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("buscar-gestao-pagamento-filtro-cliente")]
        [SwaggerGroup("GestaoPagamento")]
        public async Task<IActionResult> BuscarFiltroCliente(PaginationFilter filtro)
        {
            try
            {

                if (filtro.Filtro.Count() == 0)
                    return Response("Selecione os filtros obrigatorios", false);

                if (!ValidaFiltro(filtro, "DtHrLancamento"))
                    return Response("Selecione os filtros obrigatorios", false);

                var listaFiltroPadrao = new List<FiltroItem>();
                listaFiltroPadrao.AddRange(filtro.Filtro);
                listaFiltroPadrao.Add(new FiltroItem { FilterType = "equals", Property = "Grupo", Value = "EC" });
                listaFiltroPadrao.Add(new FiltroItem { FilterType = "equals", Property = "CliId", Value = User.Identity.Name.ToString().Descriptar() });

                filtro.Filtro = listaFiltroPadrao;

                var listaPagamentos = _mapper.Map<DtoGestaoPagamento[]>(await _gestaoPagamentoService.BuscarComFiltro(filtro));

                var totalCredito = listaPagamentos.Where(x => x.Tipo.Equals("C")).Sum(x => decimal.Parse(x.ValorFormatado, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                var totalDebito = listaPagamentos.Where(x => x.Tipo.Equals("D")).Sum(x => decimal.Parse(x.ValorFormatado, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                var total = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", totalCredito - totalDebito) ;

                return Response( new { listaPagamentos, total });
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("inserir")]
        [SwaggerGroup("GestaoPagamento")]
        public async Task<IActionResult> Inserir(DtoGestaoPagamento dto)
        {
            try
            {

                var usuarioLogado = await _usuarioService.ObterPorId(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));

                if (usuarioLogado == null)
                    return Response("Erro ao cadastrar", false);

                dto.UsuNome = usuarioLogado.Nome;
                dto.UsuCpf = usuarioLogado.CPF;
                dto.DtHrAcaoUsuario = DateTime.Now;
                dto.Grupo = "EC";
                dto.CodRef = "LANC-CLIENTE-CRED-DEB";
                dto.VlBruto = "0,00";

                var response = await _gestaoPagamentoService.Adicionar(_mapper.Map<GestaoPagamento>(dto));

                if (!response)
                    return Response("Erro ao cadastrar", false);

                return Response("Cadastro com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpDelete("excluir/{id}")]
        [SwaggerGroup("GestaoPagamento")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var buscar = await _gestaoPagamentoService.ObterPorId(id);

                if (buscar == null)
                    return Response("Erro ao excluir", false);

                var response = await _gestaoPagamentoService.Excluir(buscar);

                if (!response)
                    return Response("Erro ao excluir", false);

                return Response("Excluído com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpGet("saldo-atual")]
        [SwaggerGroup("GestaoPagamento")]

        public async Task<IActionResult> SaldoAtual()
        {
            try
            {
                var buscar = await _gestaoPagamentoService.BuscarExpressao(x => x.CliId.Equals(Convert.ToInt32(User.Identity.Name.ToString().Descriptar())) && x.Grupo.Equals("EC") && x.Status.Equals("AP"));

                var somaCredito = buscar.ToList().Where(x => x.Tipo.Equals("C")).Sum(x => decimal.Parse(x.VlLiquido, new NumberFormatInfo() { NumberDecimalSeparator = "," }));
                var somaDebito = buscar.ToList().Where(x => x.Tipo.Equals("D")).Sum(x => decimal.Parse(x.VlLiquido, new NumberFormatInfo() { NumberDecimalSeparator = "," }));

                return Response(new { saldoCliente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", somaCredito - somaDebito)  });
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpPost("solicitar-pagto-cliente")]
        [SwaggerGroup("GestaoPagamento")]
        public async Task<IActionResult> SolicitarPagtoCliente(DtoGestaoPagamento dto)
        {
            try
            {

                var buscar = await _gestaoPagamentoService.BuscarExpressao(x => x.CliId.Equals(Convert.ToInt32(User.Identity.Name.ToString().Descriptar())) && x.Grupo.Equals("EC") && x.Status.Equals("PE"));

                var somaDebito = buscar.ToList().Where(x => x.Tipo.Equals("D")).Sum(x => decimal.Parse(x.ValorSolicitadoCliente, new NumberFormatInfo() { NumberDecimalSeparator = "," }));

                var cliente = await  _clienteService.ObterPorId(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));

                var limiteCredito = string.IsNullOrEmpty(cliente.LimiteCredito) ? "0,00" : cliente.LimiteCredito;

                if ((decimal.Parse(dto.ValorSolicitadoCliente, new NumberFormatInfo() { NumberDecimalSeparator = "," }) + somaDebito) > decimal.Parse(limiteCredito, new NumberFormatInfo() { NumberDecimalSeparator = "," }))
                    return Response("Limite de crédito excedido", false);


                dto.UsuNome = "-";
                dto.UsuCpf = "-";
                dto.Descricao = "Solicitação de pagamento";
                dto.DtHrAcaoUsuario = DateTime.Now;
                dto.Grupo = "EC";
                dto.CodRef = "LANC-CLIENTE-CRED-DEB";
                dto.VlBruto = "0,00";
                dto.VlLiquido = "0,00";
                dto.DtHrSolicitacoCliente = DateTime.Now;
                dto.DtHrLancamento = DateTime.Now;
                dto.Tipo = "D";
                dto.Status = "PE";
                dto.CliId = Convert.ToInt32(User.Identity.Name.ToString().Descriptar());
                dto.FopId = 0;
                dto.RceId = 0;

                var response = await _gestaoPagamentoService.Adicionar(_mapper.Map<GestaoPagamento>(dto));

                if (!response)
                    return Response("Erro ao cadastrar", false);

                return Response("Cadastro com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }


        private static bool ValidaFiltro(PaginationFilter filtro, string valor)
        {
            return filtro.Filtro.ToList().Exists(x => x.Property.Equals(valor));
        }
    }
}
