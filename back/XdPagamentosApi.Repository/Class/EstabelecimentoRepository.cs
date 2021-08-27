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
    public class EstabelecimentoRepository: Base<Estabelecimento>, IEstabelecimentoRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public EstabelecimentoRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<Estabelecimento[]> BuscarComFiltro(PaginationFilter paginationFilter)
        {
            Expression<Func<Estabelecimento, bool>> expressionDynamic = p => p.Id != 0;

            if (paginationFilter.Filtro.Count() > 0)
                expressionDynamic = _filtroDinamico.FromFiltroItemList<Estabelecimento>(paginationFilter.Filtro.ToList());
            else
                return base.ObterTodos().Result.ToArray();

            IQueryable<Estabelecimento> query = _mySqlContext.Estabelecimentos.Where(expressionDynamic);

            if (paginationFilter.Filtro.Count() > 0)
                return await query.AsNoTracking().ToArrayAsync();

            return await query.AsNoTracking().OrderBy(c => c.Nome).ToArrayAsync();
        }
    }
}
