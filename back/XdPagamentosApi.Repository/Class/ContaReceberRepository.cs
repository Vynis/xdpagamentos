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
    public class ContaReceberRepository : Base<ContaReceber>, IContaReceberRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public ContaReceberRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<ContaReceber[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            Expression<Func<ContaReceber, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<ContaReceber>(paginationFilter.Filtro.ToList());
            else
                return await _mySqlContext.ContaRecers.Include(c => c.CentroCusto).Include(c => c.ListaFluxoCaixa).ToArrayAsync();

            IQueryable<ContaReceber> query = _mySqlContext.ContaRecers.Where(expressionDynamic).Include(c => c.CentroCusto).Include(c => c.ListaFluxoCaixa);

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().ToArrayAsync();

            return await query.AsNoTracking().OrderBy(c => c.Descricao).ToArrayAsync();
        }

        public async override Task<ContaReceber> ObterPorId(int Id)
        {
            IQueryable<ContaReceber> query = _mySqlContext.ContaRecers.Where(c => c.Id.Equals(Id)).Include(c => c.CentroCusto);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
