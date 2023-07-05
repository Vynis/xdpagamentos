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
    public class ContaPagarRepository : Base<ContaPagar>, IContaPagarRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public ContaPagarRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<ContaPagar[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            Expression<Func<ContaPagar, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<ContaPagar>(paginationFilter.Filtro.ToList());
            else
                return await _mySqlContext.ContaPagars.Include(c => c.CentroCusto).Include(c => c.ListaFluxoCaixa).Include("ListaFluxoCaixa.PlanoConta").Include("ListaFluxoCaixa.ContaCaixa").ToArrayAsync();

            IQueryable<ContaPagar> query = _mySqlContext.ContaPagars.Where(expressionDynamic).Include(c => c.CentroCusto).Include(c => c.ListaFluxoCaixa).Include("ListaFluxoCaixa.PlanoConta").Include("ListaFluxoCaixa.ContaCaixa");

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().ToArrayAsync();

            return await query.AsNoTracking().OrderBy(c => c.Descricao).ToArrayAsync();
        }

        public async override Task<ContaPagar> ObterPorId(int Id)
        {
            IQueryable<ContaPagar> query = _mySqlContext.ContaPagars.Where(c => c.Id.Equals(Id)).Include(c => c.CentroCusto);

            return await query.AsNoTracking().FirstOrDefaultAsync();

        }
    }
}
