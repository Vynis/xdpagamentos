using AutoMapper;
using EnumsNET;
using FiltrDinamico.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentoApi.Shared.Enums;
using XdPagamentoApi.Shared.Helpers;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.Shared.Extensions;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GestaoPagamentoController : BaseController
    {
        private readonly IGestaoPagamentoService _gestaoPagamentoService;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        private readonly IClienteService _clienteService;
        private readonly IFormaPagtoService _formaPagtoService;

        public GestaoPagamentoController(IGestaoPagamentoService gestaoPagamentoService, IMapper mapper, IUsuarioService usuarioService, IClienteService clienteService, IFormaPagtoService formaPagtoService)
        {
            _gestaoPagamentoService = gestaoPagamentoService;
            _mapper = mapper;
            _usuarioService = usuarioService;
            _clienteService = clienteService;
            _formaPagtoService = formaPagtoService;
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

                var listaFiltroPadrao = new List<FiltroItem>();
                listaFiltroPadrao.AddRange(filtro.Filtro);
                listaFiltroPadrao.Add(new FiltroItem { FilterType = "equals", Property = "Grupo", Value = "EC" });

                filtro.Filtro = listaFiltroPadrao;

                var retornoGestaoPagamento = _mapper.Map<DtoRetornoGestaoPagamento>( await _gestaoPagamentoService.BuscarComFiltro(filtro));

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

                var retornoGestaoPagamento = _mapper.Map<DtoRetornoGestaoPagamento>(await _gestaoPagamentoService.BuscarComFiltro(filtro));

                return Response( new { listaPagamentos = retornoGestaoPagamento.listaGestaoPagamentos, total = retornoGestaoPagamento.SaldoAtual });
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("GestaoPagamento")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                return Response(await _gestaoPagamentoService.ObterPorId(id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("buscar-relatorio-gestao-pagamento")]
        [SwaggerGroup("GestaoPagamento")]
        public async Task<IActionResult> BuscarRelatorioGestaoPagamento(PaginationFilter filtro)
        {
            try
            {
                var retornoGestaoPagamento = _mapper.Map<DtoGestaoPagamentoPorCliente[]>(await _gestaoPagamentoService.BuscarRelatorioGestaoPagamento(filtro));

                return Response(retornoGestaoPagamento);
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

                if (dto.Tipo.Equals("D"))
                {
                    decimal saldoFinal = await CalculaSaldoFinal(dto);

                    if (HelperFuncoes.FormataValorDecimal(dto.VlLiquido) > saldoFinal)
                        return Response("Valor está maior que o limite cadastrado pelo cliente.", false);
                }

                dto.UsuNome = usuarioLogado.Nome;
                dto.UsuCpf = usuarioLogado.CPF;
                dto.DtHrAcaoUsuario = DateTime.Now;
                dto.Grupo = "EC";
                dto.CodRef = "LANC-CLIENTE-CRED-DEB";
                dto.VlBruto = HelperFuncoes.ValorMoedaBRString(dto.VlBruto);
                dto.ValorSolicitadoCliente = "0,00";
                dto.VlLiquido = HelperFuncoes.ValorMoedaBRString(dto.VlLiquido);
                dto.VlVenda = HelperFuncoes.ValorMoedaBRString(dto.VlLiquido);
                dto.Status = "AP";
                dto.DtHrSolicitacoCliente = DateTime.Now;

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

        private async Task<decimal> CalculaSaldoFinal(DtoGestaoPagamento dto)
        {

            var buscarSaldoCliente = await _gestaoPagamentoService.BuscaSaldoCliente(dto.CliId);

            return HelperFuncoes.FormataValorDecimal(buscarSaldoCliente.SaldoFinal);
        }

        [HttpPut("alterar")]
        [SwaggerGroup("GestaoPagamento")]
        public async Task<IActionResult> Alterar(DtoGestaoPagamento dto)
        {
            try
            {
                var dados = await _gestaoPagamentoService.ObterPorId(dto.Id);

                if (dados == null)
                    return Response("Pagamento não encontrado", false);

                var usuarioLogado = await _usuarioService.ObterPorId(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));

                if (usuarioLogado == null)
                    return Response("Usuario não esta logado", false);

                if (dto.Tipo.Equals("D"))
                {
                    decimal saldoFinal = await CalculaSaldoFinal(dto);

                    if (HelperFuncoes.FormataValorDecimal(dto.VlLiquido) > saldoFinal)
                        return Response("Valor está maior que o limite cadastrado pelo cliente.", false);
                }

                dados.UsuNome = usuarioLogado.Nome;
                dados.UsuCpf = usuarioLogado.CPF;
                dados.DtHrAcaoUsuario = DateTime.Now;
                dados.Descricao = dto.Descricao;
                dados.DtHrLancamento = dto.DtHrLancamento;
                dados.Tipo = dto.Tipo;
                dados.VlLiquido = HelperFuncoes.ValorMoedaBRString(dto.VlLiquido);
                dados.VlBruto = HelperFuncoes.ValorMoedaBRString(dto.VlLiquido);
                dados.VlVenda = HelperFuncoes.ValorMoedaBRString(dto.VlLiquido);
                dados.FopId = dto.FopId;
                dados.CliId = dto.CliId;
                dados.RceId = dto.RceId;
                dados.Status = dto.Status;

                var response = await _gestaoPagamentoService.Atualizar(_mapper.Map<GestaoPagamento>(dados));

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

                var buscarSaldoCliente = await _gestaoPagamentoService.BuscaSaldoCliente(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));

                return Response(
                    new { 
                        saldoCliente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", buscarSaldoCliente.SaldoAtual),
                        limiteCliente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", buscarSaldoCliente.Limite), 
                        total = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", buscarSaldoCliente.SaldoFinal)  
                    });

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpGet("saldo-atual-cliente/{id}")]
        [SwaggerGroup("GestaoPagamento")]

        public async Task<IActionResult> SaldoAtual(int id)
        {
            try
            {

                var buscarSaldoCliente = await _gestaoPagamentoService.BuscaSaldoCliente(id);


                return Response(
                    new
                    {
                        saldoCliente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", buscarSaldoCliente.SaldoAtual),
                        limiteCliente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", buscarSaldoCliente.Limite),
                        total = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", buscarSaldoCliente.SaldoFinal)
                    });

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
                if (dto.DtAgendamento == null)
                    return Response("Data agendamento invalida. Informe a data posterior no dia de hoje. ", false);

                if (dto.DtAgendamento?.Date <= DateTime.Now.Date)
                    return Response("Data agendamento invalida. Informe a data posterior no dia de hoje. ", false);


                dto.CliId = Convert.ToInt32(User.Identity.Name.ToString().Descriptar());

                decimal saldoFinal = await CalculaSaldoFinal(dto);

                if (HelperFuncoes.FormataValorDecimal(dto.ValorSolicitadoCliente) > saldoFinal)
                    return Response("Valor solicitado está maior que limite disponível.", false);

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
                dto.RceId = 0;
                dto.ValorSolicitadoCliente = HelperFuncoes.ValorMoedaBRString(dto.ValorSolicitadoCliente);


                var formaPagamentoSelecionado = await _formaPagtoService.ObterPorId(dto.FopId);
                var clienteLgado = await _clienteService.ObterPorId(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()));
                var tipoContaTxt = clienteLgado.TipoConta == "P" ? "Poupança" : "Conta Corrente";

                switch (dto.FopId)
                {
                    case 2:
                        dto.MeioPagamento = clienteLgado.BanId == 0 ? formaPagamentoSelecionado.Descricao : $"{formaPagamentoSelecionado.Descricao} ({tipoContaTxt}): Banco: {clienteLgado.Banco.Numero} - {clienteLgado.Banco.Nome} | Ag: {clienteLgado.NumAgencia} | Conta: {clienteLgado.NumConta}";
                        break;
                    case 5:
                        dto.MeioPagamento = clienteLgado.TipoChavePix == null ? formaPagamentoSelecionado.Descricao : $"{formaPagamentoSelecionado.Descricao} ({((TiposChavePix)clienteLgado.TipoChavePix).AsString(EnumFormat.Description)}): {clienteLgado.ChavePix} ";
                        break;
                    default:
                        dto.MeioPagamento = formaPagamentoSelecionado.Descricao;
                        break;
                }

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
