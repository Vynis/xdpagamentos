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

                var dadosCliente = Convert.ToInt32(filtro.Filtro.Where(x => x.Property.Equals("CliId")).FirstOrDefault().Value);

                //Saldo Atual
                var dadosGeral = await _gestaoPagamentoService.BuscarExpressao(x => x.CliId.Equals(dadosCliente) && x.Grupo.Equals("EC") && x.Status.Equals("AP"));

                retornoGestaoPagamento.SaldoAtual = HelperFuncoes.ValorMoedaBRDecimal(dadosGeral.Where(x => x.Tipo.Equals("C")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido)) - dadosGeral.Where(x => x.Tipo.Equals("D")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido)));

                //Saldo Anterior
                var dataHrLancamento = filtro.Filtro.Where(x => x.Property.Equals("DtHrLancamento") && x.FilterType.Equals("greaterThanEquals")).FirstOrDefault().Value.ToString();

                var dadosSaldoAnterior = await _gestaoPagamentoService.BuscarExpressao(x => x.DtHrLancamento < DateTime.Parse(dataHrLancamento) && x.CliId.Equals(dadosCliente) && x.Status.Equals("AP"));

                retornoGestaoPagamento.SaldoAnterior = HelperFuncoes.ValorMoedaBRDecimal(dadosSaldoAnterior.Where(x => x.Tipo.Equals("C")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido)) - dadosSaldoAnterior.Where(x => x.Tipo.Equals("D")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido)));

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

                var totalCredito = listaPagamentos.Where(x => x.Tipo.Equals("C")).Sum(x => HelperFuncoes.FormataValorDecimal(x.ValorFormatado));
                var totalDebito = listaPagamentos.Where(x => x.Tipo.Equals("D")).Sum(x => HelperFuncoes.FormataValorDecimal(x.ValorFormatado));
                var total = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", totalCredito - totalDebito) ;

                return Response( new { listaPagamentos, total });
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
                dto.VlBruto = "0,00";
                dto.ValorSolicitadoCliente = "0,00";
                dto.VlLiquido = HelperFuncoes.ValorMoedaBRString(dto.VlLiquido);
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
            //Limite de Crecito
            var dadosCliente = await _clienteService.ObterPorId(dto.CliId);

            var limiteCredito = HelperFuncoes.FormataValorDecimal(dadosCliente.LimiteCredito);

            //Saldo Atual
            var dadosGeral = await _gestaoPagamentoService.BuscarExpressao(x => x.CliId.Equals(dto.CliId) && x.Grupo.Equals("EC") && x.Status.Equals("AP"));

            var saldoAtual = dadosGeral.Where(x => x.Tipo.Equals("C")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido)) - dadosGeral.Where(x => x.Tipo.Equals("D")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido));

            return saldoAtual + limiteCredito;
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
                var buscar = await _gestaoPagamentoService.BuscarExpressao(x => x.CliId.Equals(Convert.ToInt32(User.Identity.Name.ToString().Descriptar())) && x.Grupo.Equals("EC") && x.Status.Equals("AP"));

                var somaCredito = buscar.ToList().Where(x => x.Tipo.Equals("C")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido));
                var somaDebito = buscar.ToList().Where(x => x.Tipo.Equals("D")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido));
                var limiteCliente =HelperFuncoes.FormataValorDecimal(HelperFuncoes.ValorMoedaBRString((await _clienteService.ObterPorId(Convert.ToInt32(User.Identity.Name.ToString().Descriptar()))).LimiteCredito));

                return Response(
                    new { 
                        saldoCliente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", somaCredito - somaDebito),
                        limiteCliente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", limiteCliente), 
                        total = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", (somaCredito - somaDebito) + limiteCliente)  
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
                var buscar = await _gestaoPagamentoService.BuscarExpressao(x => x.CliId.Equals(id) && x.Grupo.Equals("EC") && x.Status.Equals("AP"));

                var somaCredito = buscar.ToList().Where(x => x.Tipo.Equals("C")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido));
                var somaDebito = buscar.ToList().Where(x => x.Tipo.Equals("D")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido));
                var limiteCliente = HelperFuncoes.FormataValorDecimal(HelperFuncoes.ValorMoedaBRString((await _clienteService.ObterPorId(id)).LimiteCredito));

                return Response(
                    new
                    {
                        saldoCliente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", somaCredito - somaDebito),
                        limiteCliente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", limiteCliente),
                        total = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", (somaCredito - somaDebito) + limiteCliente)
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
