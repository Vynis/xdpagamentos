using FiltrDinamico.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

            var retorno = await query.AsNoTracking().ToArrayAsync();

            foreach(var ret in retorno)
            {
                if (ret.CodRef.Contains("ORPID"))
                {
                    var codRef = ret.CodRef.Replace("ORPID", "");

                    var pagamentos = await _mySqlContext.Pagamentos.Where(x => x.OrpId == Convert.ToInt32(codRef)).FirstOrDefaultAsync();

                    var transacao = await _mySqlContext.Transacoes.Where(x => x.PagId == pagamentos.Id).FirstOrDefaultAsync();

                    if (transacao != null)
                    {
                        ret.VlBrutoTransacao = transacao.VlBruto;
                        ret.QtdParcelaTransacao = transacao.QtdParcelas;
                        ret.CodAutorizacaoTransacao = transacao.CodAutorizacao;
                        ret.NumCartaoTransacao = transacao.NumCartao;
                        ret.MeioCapturaTransacao = transacao.MeioCaptura;
                        ret.TipoOperacaoTransacao = transacao.Descricao;
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
