using FiltrDinamico.Core;
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

        public async Task<GestaoPagamento[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            Expression<Func<GestaoPagamento, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<GestaoPagamento>(paginationFilter.Filtro.ToList());
            else
                return base.ObterTodos().Result.ToArray();

            IQueryable<GestaoPagamento> query = _mySqlContext.GestaoPagamentos.Where(expressionDynamic).Include(c => c.Cliente).Include(c => c.RelContaEstabelecimento).Include("RelContaEstabelecimento.Estabelecimento");

            return await query.AsNoTracking().ToArrayAsync();

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

        public async override Task<IEnumerable<GestaoPagamento>> BuscarExpressao(Expression<Func<GestaoPagamento, bool>> predicado)
        {

            IQueryable<GestaoPagamento> query = _mySqlContext.GestaoPagamentos.Where(predicado).Include(c => c.Cliente);

            return await query.AsNoTracking().ToListAsync();

        }
    }
}
