using AutoMapper;
using FiltrDinamico.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Helpers;
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

                if (!ValidaFiltro(filtro, "DtHrLancamento") || !ValidaFiltro(filtro, "RelContaEstabelecimento.CocId"))
                    return Response("Selecione os filtros obrigatorios", false);

                var retornoGestaoPagamento = new DtoRetornoGestaoExtrato();

                var listaFiltroPadrao = new List<FiltroItem>();
                listaFiltroPadrao.AddRange(filtro.Filtro);

                filtro.Filtro = listaFiltroPadrao;

                var listaPagamentos = _mapper.Map<DtoGestaoPagamento[]>(await _gestaoPagamentoService.BuscarComFiltro(filtro));

                var listaPagamentoFormatado = listaPagamentos.ToList().Where(x => x.Grupo.Equals("EG") || !x.VlBruto.Equals("0,00")).ToArray();

                retornoGestaoPagamento.listaGestaoPagamentos = listaPagamentoFormatado;

                if (listaPagamentoFormatado.Count() == 0)
                    return Response(retornoGestaoPagamento);

                var dadosConta = listaPagamentoFormatado.FirstOrDefault().RceId;

                //Saldo Atual
                var dadosGeral = await _gestaoPagamentoService.BuscarExpressao(x => x.RceId.Equals(dadosConta) && (x.Grupo.Equals("EG") || !x.VlBruto.Equals("0,00")) );

                retornoGestaoPagamento.SaldoAtual = HelperFuncoes.ValorMoedaBRDecimal(dadosGeral.Where(x => x.Tipo.Equals("C")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlBruto)) - dadosGeral.Where(x => x.Tipo.Equals("D")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlBruto)));

                //Saldo Anterior
                var dataHrLancamento = filtro.Filtro.Where(x => x.Property.Equals("DtHrLancamento") && x.FilterType.Equals("greaterThanEquals")).FirstOrDefault().Value?.ToString();

                if (dataHrLancamento != null)
                {
                    var dadosSaldoAnterior = await _gestaoPagamentoService.BuscarExpressao(x => x.DtHrLancamento < DateTime.Parse(dataHrLancamento) && x.RceId.Equals(dadosConta));

                    retornoGestaoPagamento.SaldoAnterior = HelperFuncoes.ValorMoedaBRDecimal(dadosSaldoAnterior.Where(x => x.Tipo.Equals("C")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlBruto)) - dadosSaldoAnterior.Where(x => x.Tipo.Equals("D")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlBruto)));
                }
                else
                {
                    retornoGestaoPagamento.SaldoAnterior = "0,00";
                }

              
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
                dto.ValorSolicitadoCliente = "0,00";
                dto.VlBruto = HelperFuncoes.ValorMoedaBRString(dto.VlBruto);
                dto.Status = "AP";

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
