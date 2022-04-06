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
    public class RelatoriosRepository : Base<object>, IRelatoriosRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public RelatoriosRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<VwRelatorioSaldoCliente[]> BuscaRelatorioSaldoCliente(PaginationFilter paginationFilter)
        {
            Expression<Func<VwRelatorioSaldoCliente, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<VwRelatorioSaldoCliente>(paginationFilter.Filtro.ToList());
            else
                return await _mySqlContext.VwRelatorioSaldoClientes.ToArrayAsync();

            IQueryable<VwRelatorioSaldoCliente> query = _mySqlContext.VwRelatorioSaldoClientes.Where(expressionDynamic);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<VwRelatorioSaldoContaCorrente[]> BuscaRelatorioSaldoContaCorrente(PaginationFilter paginationFilter)
        {
            Expression<Func<VwRelatorioSaldoContaCorrente, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<VwRelatorioSaldoContaCorrente>(paginationFilter.Filtro.ToList());
            else
                return await _mySqlContext.VwRelatorioSaldoContaCorrentes.ToArrayAsync();

            IQueryable<VwRelatorioSaldoContaCorrente> query = _mySqlContext.VwRelatorioSaldoContaCorrentes.Where(expressionDynamic);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<VwRelatorioSolicitacao[]> BuscaRelatorioSolicitacao(PaginationFilter paginationFilter)
        {
            Expression<Func<VwRelatorioSolicitacao, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<VwRelatorioSolicitacao>(paginationFilter.Filtro.ToList());
            else
                return await _mySqlContext.VwRelatorioSolicitacoes.ToArrayAsync();

            IQueryable<VwRelatorioSolicitacao> query = _mySqlContext.VwRelatorioSolicitacoes.Where(expressionDynamic);

            return await query.AsNoTracking().ToArrayAsync();
        }
    }
}
