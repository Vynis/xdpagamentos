using FiltrDinamico.Core;
using FiltrDinamico.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Helpers;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;

namespace XdPagamentosApi.Repository.Class
{
    public class GestaoPagamentoRepository : Base<GestaoPagamento>, IGestaoPagamentoRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public GestaoPagamentoRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<RetGestaoPagamentoTransacoes> BuscarComFiltro(PaginationFilter paginationFilter)
        {

            var retornoGestaopagamentoTransacoes = new RetGestaoPagamentoTransacoes();

            Expression<Func<VwGestaoPagamentoTransacoes, bool>> expressionDynamic = _filtroDinamico.FromFiltroItemList<VwGestaoPagamentoTransacoes>(paginationFilter.Filtro.ToList());

            //Lista de Gestao de Pagamentos
            IQueryable<VwGestaoPagamentoTransacoes> query = _mySqlContext.VwGestaoPagamentoTransacao.Where(expressionDynamic).Include(c => c.Cliente).Include(c => c.RelContaEstabelecimento).Include("RelContaEstabelecimento.Estabelecimento");

            retornoGestaopagamentoTransacoes.listaGestaoPagamentos = await query.AsNoTracking().ToArrayAsync();

            //Saldo Atual
            var cliId = Convert.ToInt32(paginationFilter.Filtro.Where(x => x.Property.Equals("CliId")).FirstOrDefault().Value);

            IQueryable<VwRelatorioSaldoCliente> querySaldoCliente = _mySqlContext.VwRelatorioSaldoClientes.Where(x => x.Id == cliId);

            var retornoViewSaldoCliente = await querySaldoCliente.AsNoTracking().FirstOrDefaultAsync();

            retornoGestaopagamentoTransacoes.SaldoAtual = retornoViewSaldoCliente.SaldoAtual;

            //Saldo Anterior
            var listaFiltroPadrao = new List<FiltroItem>();
            listaFiltroPadrao.Add(new FiltroItem { FilterType = "equals", Property = "Grupo", Value = "EC" });
            listaFiltroPadrao.Add(new FiltroItem { FilterType = "equals", Property = "Status", Value = "AP" });
            listaFiltroPadrao.Add(new FiltroItem { FilterType = "equals", Property = "CliId", Value = cliId });
            listaFiltroPadrao.Add(new FiltroItem { FilterType = "lessThanEquals", Property = "DtHrLancamento", Value = paginationFilter.Filtro.Where(x => x.Property.Equals("DtHrLancamento") && x.FilterType.Equals("greaterThanEquals")).FirstOrDefault().Value.ToString() });

            paginationFilter.Filtro = listaFiltroPadrao;

            expressionDynamic = _filtroDinamico.FromFiltroItemList<VwGestaoPagamentoTransacoes>(paginationFilter.Filtro.ToList());

            IQueryable<VwGestaoPagamentoTransacoes> querySaldoAnterior = _mySqlContext.VwGestaoPagamentoTransacao.Where(expressionDynamic);

            var retornoSadoAnterio = await querySaldoAnterior.AsNoTracking().ToArrayAsync();

            retornoGestaopagamentoTransacoes.SaldoAnterior = HelperFuncoes.ValorMoedaBRDecimal(retornoSadoAnterio.Where(x => x.Tipo.Equals("C")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquidoCliente)) - retornoSadoAnterio.Where(x => x.Tipo.Equals("D")).Sum(x => HelperFuncoes.FormataValorDecimal(x.VlLiquido)));

            return retornoGestaopagamentoTransacoes;
        }

        public async Task<GestaoPagamento[]> BuscarComFiltroExtrato(PaginationFilter paginationFilter)
        {
            Expression<Func<GestaoPagamento, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<GestaoPagamento>(paginationFilter.Filtro.ToList());
            else
                return base.ObterTodos().Result.ToArray();

            IQueryable<GestaoPagamento> query = _mySqlContext.GestaoPagamentos.Where(expressionDynamic).Include(c => c.Cliente).Include(c => c.RelContaEstabelecimento).Include("RelContaEstabelecimento.Estabelecimento");

            var retorno = await query.AsNoTracking().ToArrayAsync();

            return retorno;
        }

        public async Task<GestaoPagamento[]> BuscarComFiltroCliente(PaginationFilter paginationFilter)
        {
            Expression<Func<GestaoPagamento, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<GestaoPagamento>(paginationFilter.Filtro.ToList());
            else
                return base.ObterTodos().Result.ToArray();

            IQueryable<GestaoPagamento> query = _mySqlContext.GestaoPagamentos.Where(expressionDynamic).Include(c => c.Cliente).Include(c => c.RelContaEstabelecimento).Include("RelContaEstabelecimento.Estabelecimento");

            var retorno = await query.AsNoTracking().ToArrayAsync();

            foreach (var ret in retorno)
            {
                if (ret.CodRef.Contains("ORPID"))
                {
                    var codRef = ret.CodRef.Replace("ORPID", "");

                    var transacao = await _mySqlContext.Transacoes.Where(x => x.Pagamentos.OrpId == Convert.ToInt32(codRef)).Include(c => c.Pagamentos).FirstOrDefaultAsync();

                    if (transacao != null)
                    {
                        ret.VlBrutoTransacao = transacao.VlBruto;
                        ret.QtdParcelaTransacao = transacao.QtdParcelas;
                        ret.CodAutorizacaoTransacao = transacao.CodAutorizacao;
                        ret.NumCartaoTransacao = transacao.NumCartao;
                        ret.MeioCapturaTransacao = transacao.MeioCaptura;
                        ret.TipoOperacaoTransacao = transacao.Descricao;

                        var novoValorLiquido = HelperFuncoes.FormataValorDecimal(ret.VlBrutoTransacao) - (HelperFuncoes.FormataValorDecimal(ret.VlBrutoTransacao) * HelperFuncoes.FormataValorDecimal(transacao.TitPercDesconto) / 100);
                        ret.VlLiquido = HelperFuncoes.ValorMoedaBRDecimal(novoValorLiquido);
                    }
                }
            }

            return retorno;
        }

        public async Task<VwRelatorioSaldoCliente> BuscaSaldoCliente(int cliId)
        {
            IQueryable<VwRelatorioSaldoCliente> query = _mySqlContext.VwRelatorioSaldoClientes.Where(x => x.Id == cliId);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<GestaoPagamento>> BuscarExpressao(Expression<Func<GestaoPagamento, bool>> predicado)
        {

            IQueryable<GestaoPagamento> query = _mySqlContext.GestaoPagamentos.Where(predicado).Include(c => c.Cliente);

            return await query.AsNoTracking().ToListAsync();

        }
    }
}
