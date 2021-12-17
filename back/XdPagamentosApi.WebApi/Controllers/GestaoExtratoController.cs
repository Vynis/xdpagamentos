using AutoMapper;
using FiltrDinamico.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    public class GestaoExtratoController : BaseController
    {
        private readonly IGestaoPagamentoService _gestaoPagamentoService;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;

        public GestaoExtratoController(IGestaoPagamentoService gestaoPagamentoService, IMapper mapper, IUsuarioService usuarioService)
        {
            _gestaoPagamentoService = gestaoPagamentoService;
            _mapper = mapper;
            _usuarioService = usuarioService;
        }

        [HttpPost("buscar-gestao-extrato-filtro")]
        [SwaggerGroup("GestaoExtrato")]
        public async Task<IActionResult> BuscarFiltro(PaginationFilter filtro)
        {
            try
            {

                if (filtro.Filtro.Count() == 0)
                    return Response("Selecione os filtros obrigatorios", false);

                if (!ValidaFiltro(filtro, "DtHrLancamento") || !ValidaFiltro(filtro, "CliId"))
                    return Response("Selecione os filtros obrigatorios", false);

                var retornoGestaoPagamento = new DtoRetornoGestaoPagamento();

                var listaFiltroPadrao = new List<FiltroItem>();
                listaFiltroPadrao.AddRange(filtro.Filtro);
                listaFiltroPadrao.Add(new FiltroItem { FilterType = "equals", Property = "Grupo", Value = "EG" });

                filtro.Filtro = listaFiltroPadrao;

                var listaPagamentos = _mapper.Map<DtoGestaoPagamento[]>(await _gestaoPagamentoService.BuscarComFiltro(filtro));

                retornoGestaoPagamento.listaGestaoPagamentos = listaPagamentos;

                if (listaPagamentos.Count() == 0)
                    return Response(retornoGestaoPagamento);

                var dadosConta = listaPagamentos.FirstOrDefault().RceId;

                //Saldo Atual
                var dadosGeral = await _gestaoPagamentoService.BuscarExpressao(x => x.RceId.Equals(dadosConta));

                retornoGestaoPagamento.SaldoAtual = (dadosGeral.Where(x => x.Tipo.Equals("C")).Sum(x => Convert.ToDecimal(x.VlLiquido)) - dadosGeral.Where(x => x.Tipo.Equals("D")).Sum(x => Convert.ToDecimal(x.VlLiquido))).ToString();

                //Saldo Anterior
                var dataHrLancamento = filtro.Filtro.Where(x => x.Property.Equals("DtHrLancamento") && x.FilterType.Equals("greaterThanEquals")).FirstOrDefault().Value.ToString();

                var dadosSaldoAnterior = await _gestaoPagamentoService.BuscarExpressao(x => x.DtHrLancamento < DateTime.Parse(dataHrLancamento) && x.CliId.Equals(dadosConta));

                retornoGestaoPagamento.SaldoAnterior = (dadosSaldoAnterior.Where(x => x.Tipo.Equals("C")).Sum(x => Convert.ToDecimal(x.VlLiquido)) - dadosSaldoAnterior.Where(x => x.Tipo.Equals("D")).Sum(x => Convert.ToDecimal(x.VlLiquido))).ToString();

                return Response(retornoGestaoPagamento);
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpPost("inserir")]
        [SwaggerGroup("GestaoExtrato")]
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
                dto.Grupo = "EG";
                dto.CodRef = "LANC-EXTRATO-CRED-DEB";
                dto.VlLiquido = "0,00";

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
