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

            IQueryable<GestaoPagamento> query = _mySqlContext.GestaoPagamentos.Where(expressionDynamic).Include(c => c.Cliente);

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().ToArrayAsync();


            return await query.AsNoTracking().ToArrayAsync();
        }
    }
}
